// <copyright file="Spreadsheet.cs" company="Benjamin Michaelis">
// Copyright (c) Benjamin Michaelis. ID: 11620581. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;

namespace SpreadsheetEngine
{
    /// <summary>
    /// Is a container of a 2D array of cells.
    /// </summary>
    public partial class Spreadsheet
    {
        private EventHandler<BeforeCellChangedEventArgs> BeforeCellPropertyChangedEventHandler { get; init; }

        /// <summary>
        /// Event handler to connect to UI level.
        /// </summary>
        public event PropertyChangedEventHandler? OnCellPropertyChanged;

        private Stack<SpreadsheetCell> UndoStack { get; } = new();

        private SpreadsheetCell[,] CellsOfSpreadsheet { get; set; }

        private XDocument? SrcTree { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Spreadsheet"/> class.
        /// </summary>
        /// <param name="columns">Number of columns.</param>
        /// <param name="rows">Number of rows.</param>
        public Spreadsheet(int columns, int rows)
        {
            this.BeforeCellPropertyChangedEventHandler = new EventHandler<BeforeCellChangedEventArgs>(this.BeforeCellPropertyChanged);
            this.InitializeSpreadsheet(columns, rows);
        }

        /// <summary>
        /// Converts the first letters in a string to numbers.
        /// </summary>
        /// <param name="cellName">The name of the cell (ex: AA33).</param>
        /// <returns>Int of SpreadsheetCell.</returns>
        public int ColumnLetterToInt(string cellName)
        {
            string columnLetters = string.Concat(cellName.TakeWhile(char.IsLetter));
            int columnLocation = columnLetters.ToCharArray().Select(c => c - 'A' + 1).Reverse().Select((v, i) => v * (int)Math.Pow(26, i)).Sum();
            return columnLocation;
        }

        /// <summary>
        /// Runs a demo of the code with 50 cells displaying "Hello World".
        /// </summary>
        public void Demo()
        {
            Random random = new Random();

            for (int i = 0; i < 50; i++)
            {
                int randomCol = random.Next(0, 25);
                int randomRow = random.Next(0, 49);

                SpreadsheetCell temp = (SpreadsheetCell)this[randomCol!, randomRow!];
                temp!.Text = "Hello World";
                this.CellsOfSpreadsheet[randomRow, randomCol] = temp;
            }

            for (int i = 0; i < 50; i++)
            {
                this.CellsOfSpreadsheet[i, 1].Text = $"This is SpreadsheetCell B{i + 1}";
            }
        }

        /// <summary>
        /// Notifies for  when any property for any cell in the worksheet has changed.
        /// </summary>
        /// <param name="sender">Object that called object.</param>
        /// <param name="e">The property changed arg.</param>
        public void CellPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (sender is SpreadsheetCell evaluatingCell)
            {
                switch (e.PropertyName)
                {
                    case nameof(Cell.Text):
                        if (!string.IsNullOrEmpty(evaluatingCell.Text))
                        {
                            this.OnCellPropertyChanged?.Invoke(sender, e);
                        }

                        break;
                    case nameof(Cell.Value):
                        if (!string.IsNullOrEmpty(evaluatingCell.Value))
                        {
                            this.OnCellPropertyChanged?.Invoke(sender, e);
                        }

                        break;
                    case nameof(Cell.BackgroundColor):
                        this.OnCellPropertyChanged?.Invoke(sender, e);
                        break;
                    default:
                        throw new NotImplementedException($"Spreadsheet cell property '{e.PropertyName}' changed not implemented in spreadsheet");
                }
            }
        }

        /// <summary>
        /// Saves the spreadsheet given a specified path.
        /// </summary>
        /// <param name="savePath">Path to save the document at.</param>
        public void SaveSpreadsheet(string savePath)
        {
            this.SaveSpreadsheet();
            this.SrcTree.Save(savePath);
        }

        /// <summary>
        /// Saves the spreadsheet given a specified path.
        /// </summary>
        /// <param name="saveStream">Path to save the document at.</param>
        public void SaveSpreadsheet(System.IO.Stream saveStream)
        {
            this.SaveSpreadsheet();
            this.SrcTree.Save(saveStream);
        }

        /// <summary>
        /// loads the spreadsheet given a specified path.
        /// </summary>
        /// <param name="savePath">Path to save the document at.</param>
        public void LoadSpreadsheet(string savePath)
        {
            XDocument doc = XDocument.Load(savePath);
            this.LoadSpreadsheet(doc);
        }

        /// <summary>
        /// loads the spreadsheet given a specified path.
        /// </summary>
        /// <param name="saveStream">Path to save the document at.</param>
        public void LoadSpreadsheet(System.IO.Stream saveStream)
        {
            this.InitializeSpreadsheet(this.ColumnCount, this.RowCount);
            XDocument doc = XDocument.Load(saveStream);
            this.LoadSpreadsheet(doc);
        }

        private void SaveSpreadsheet()
        {
            this.SrcTree = new XDocument();
            var xmlTree = new XElement(nameof(Spreadsheet));
            foreach (SpreadsheetCell spreadsheetCell in this.CellsOfSpreadsheet)
            {
                if (spreadsheetCell.Text != string.Empty || spreadsheetCell.BackgroundColor != 0xFFFFFFFF)
                {
                    xmlTree.Add(new XElement(
                        nameof(SpreadsheetCell),
                        new XElement(nameof(Cell.BackgroundColor), spreadsheetCell.BackgroundColor.ToString()),
                        new XElement(nameof(Cell.Text), spreadsheetCell.Text),
                        new XAttribute(nameof(Cell.IndexName), spreadsheetCell.IndexName)));
                }
            }

            this.SrcTree.Add(xmlTree);
        }

        private void LoadSpreadsheet(XDocument doc)
        {
            this.SrcTree = doc;
            if (this.SrcTree.Root is null)
            {
                return;
            }

            switch (this.SrcTree.Root.Name.ToString())
            {
                case nameof(Spreadsheet):
                {
                    IEnumerable<XElement> spreadsheetCells = this.SrcTree.Root.Elements(nameof(SpreadsheetCell));
                    foreach (XElement cell in spreadsheetCells)
                    {
                        if (cell.FirstAttribute is null)
                        {
                            break;
                        }

                        string cellName = cell.FirstAttribute.Value;
                        bool colorParseSuccessful = uint.TryParse(
                            cell.Element(nameof(Cell.BackgroundColor))?.Value,
                            out uint cellBackgroundColor);
                        string? cellText = cell.Element(nameof(Cell.Text))?.Value;
                        if (cellText is { })
                        {
                            if (cellText != string.Empty)
                            {
                                this[cellName].Text = cellText;
                            }
                        }

                        switch (colorParseSuccessful)
                        {
                            case true:
                            {
                                if (cellBackgroundColor != 0xFFFFFFFF)
                                {
                                    this[cellName].BackgroundColor = cellBackgroundColor;
                                }

                                break;
                            }
                        }
                    }

                    break;
                }
            }
        }

        private void InitializeSpreadsheet(int columns, int rows)
        {
            this.CellsOfSpreadsheet = new SpreadsheetCell[columns, rows];
            for (int rowNum = 0; rowNum < rows; rowNum++)
            {
                for (int colNum = 0; colNum < columns; colNum++)
                {
                    this.CellsOfSpreadsheet[colNum, rowNum] = new SpreadsheetCell(colNum, rowNum, this);
                    this.CellsOfSpreadsheet[colNum, rowNum].PropertyChanged += this.CellPropertyChanged;
                    this.CellsOfSpreadsheet[colNum, rowNum].BeforePropertyChanged += this.BeforeCellPropertyChangedEventHandler;
                }
            }
        }

        private void BeforeCellPropertyChanged(object? sender, BeforeCellChangedEventArgs e)
        {
            this.UndoStack.Push((SpreadsheetCell)e.CellBeforeChange);
        }

        /// <summary>
        /// Get the cell from the cell name.
        /// </summary>
        /// <param name="cellName">Letter/number combo of cell.</param>
        /// <returns>Returns cell in spreadsheet that matches the index of the cell.</returns>
        private SpreadsheetCell? this[string cellName]
        {
            get
            {
                int columnLocation = this.ColumnLetterToInt(cellName);
                string rowLocationString = string.Join(null, System.Text.RegularExpressions.Regex.Split(cellName, "[^\\d]"));
                int rowLocation = int.Parse(rowLocationString);
                return (SpreadsheetCell)this[columnLocation - 1, rowLocation - 1];
            }
        }

        /// <summary>
        /// Gets number of columns in spreadsheet.
        /// </summary>
        public int ColumnCount => this.CellsOfSpreadsheet.GetLength(0);

        /// <summary>
        /// Gets number of rows in spreadsheet.
        /// </summary>
        public int RowCount => this.CellsOfSpreadsheet.GetLength(1);

        /// <summary>
        /// Indexer to pass back a Spreadsheet Cell.
        /// </summary>
        /// <param name="columnIndex">The column of the location of the cell.</param>
        /// <param name="rowIndex">The row of the location of the cell.</param>
        /// <returns>A Cell.</returns>
        public Cell this[int columnIndex, int rowIndex] => this.CellsOfSpreadsheet[columnIndex, rowIndex];

        /// <summary>
        /// Allows undo of a command.
        /// </summary>
        public void Undo()
        {
            if (this.UndoStack.Count > 0)
            {
                SpreadsheetCell cellsPreviousState = this.UndoStack.Pop();
                this.CellsOfSpreadsheet[cellsPreviousState.ColumnIndex, cellsPreviousState.RowIndex]
                    .BeforePropertyChanged -= this.BeforeCellPropertyChangedEventHandler;
                this.CellsOfSpreadsheet[cellsPreviousState.ColumnIndex, cellsPreviousState.RowIndex].Text = cellsPreviousState.Text;
                this.CellsOfSpreadsheet[cellsPreviousState.ColumnIndex, cellsPreviousState.RowIndex].BackgroundColor = cellsPreviousState.BackgroundColor;
                this.CellsOfSpreadsheet[cellsPreviousState.ColumnIndex, cellsPreviousState.RowIndex].ErrorMessage = cellsPreviousState.ErrorMessage;
                this.CellsOfSpreadsheet[cellsPreviousState.ColumnIndex, cellsPreviousState.RowIndex]
                    .BeforePropertyChanged += this.BeforeCellPropertyChangedEventHandler;
            }
        }
    }
}

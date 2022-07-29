// <copyright file="Spreadsheet.cs" company="Benjamin Michaelis">
// Copyright (c) Benjamin Michaelis. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;

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

        /// <summary>
        /// Gets or sets list containing the cells that are currently being calculated.
        /// </summary>
        private HashSet<SpreadsheetCell> IsCalculating { get; set; } = new();

        /// <summary>
        /// Gets list containing the cells that are currently being calculated.
        /// </summary>
        private IEnumerable<SpreadsheetCell> IsErrored
        {
            get => this.CellsOfSpreadsheet.Cast<SpreadsheetCell>().Where(item => item.IsErrored);
        }

        private Stack<Command> UndoStack { get; } = new();
        private Stack<Command> RedoStack { get; } = new();

        private SpreadsheetCell[,] CellsOfSpreadsheet { get; set; } = null!;

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
        /// Runs a demo of the code with 50 cells displaying "Hello World".
        /// </summary>
        public void Demo()
        {
            Random random = new();

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
                            this.IsCalculating.Clear();
                        }

                        break;
                    case nameof(Cell.Value):
                        if (!string.IsNullOrEmpty(evaluatingCell.Value))
                        {
                            this.OnCellPropertyChanged?.Invoke(sender, e);
                            this.IsCalculating.Clear();
                        }

                        break;
                    case nameof(Cell.ErrorMessage):
                        this.OnCellPropertyChanged?.Invoke(sender, e);
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

        /// <summary>
        /// Set a group of cells background color.
        /// </summary>
        /// <param name="cells">The cells to change the background of.</param>
        /// <param name="color">The color to change the cells color to.</param>
        public void SetBackgroundColor(IEnumerable<Cell> cells, uint color)
        {
            List<Cell> clonedCells = new();
            foreach (Cell cell in cells)
            {
                clonedCells.Add(cell.Clone());
                cell.BeforePropertyChanged -= this.BeforeCellPropertyChangedEventHandler;
                cell.BackgroundColor = color;
                cell.BeforePropertyChanged += this.BeforeCellPropertyChangedEventHandler;
            }

            Command undoCommand = new($"Undo cell background color change", clonedCells.ToArray());
            this.UndoStack.Push(undoCommand);
            this.RedoStack.Clear();
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
                        this.UndoStack.Clear();
                        this.RedoStack.Clear();
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
            this.UndoStack.Push(new Command(e.Description, e.CellsBeforeChange));
            this.RedoStack.Clear();
        }

        /// <summary>
        /// Get the cell from the cell name.
        /// </summary>
        /// <param name="cellName">Letter/number combo of cell.</param>
        /// <returns>Returns cell in spreadsheet that matches the index of the cell.</returns>
        private SpreadsheetCell this[string cellName]
        {
            get
            {
                int columnLocation = Cell.ColumnLetterToInt(cellName);
                string rowLocationString = string.Join(null, System.Text.RegularExpressions.Regex.Split(cellName, "[^\\d]"));
                int rowLocation = int.Parse(rowLocationString);
                return (SpreadsheetCell)this[columnLocation - 1, rowLocation - 1];
            }
        }

        /// <summary>
        /// Check to see if a cell name is valid.
        /// </summary>
        /// <param name="cellName">The name of the cell.</param>
        /// <returns>Returns true if the cell name is valid and can be obtained, false if not.</returns>
        public bool IsValidCellName(string cellName)
        {
            // https://regexr.com/5r9fa
            Regex lettersThenNumbersRegex = new(@"[A-Za-z]+\d+$");
            bool isValidCellName = lettersThenNumbersRegex.IsMatch(cellName);
            int columnLocation = Cell.ColumnLetterToInt(cellName);
            string rowLocationString = string.Join(null, System.Text.RegularExpressions.Regex.Split(cellName, "[^\\d]"));
            if (int.TryParse(rowLocationString, out int rowLocation) && isValidCellName)
            {
                return this.IsValidIndex(columnLocation - 1, rowLocation - 1);
            }

            return false;
        }

        /// <summary>
        /// Try get the cell at the specified location.
        /// </summary>
        /// <param name="cellName">The name of the cell location.</param>
        /// <param name="cell">The desired cell, returns default if not obtained.</param>
        /// <returns>Returns true is desired cell is obtained, false if not.</returns>
        public bool TryGetCell(string cellName, out Cell? cell)
        {
            cell = default;
            if (this.IsValidCellName(cellName))
            {
                cell = this[cellName];
                return true;
            }

            return false;
        }

        /// <summary>
        /// Checks if the desired row and column are within the bounds of the current spreadsheet.
        /// </summary>
        /// <param name="columnIndex">The desired column.</param>
        /// <param name="rowIndex">The desired row.</param>
        /// <returns>Returns true if cell could be obtained at the specified location, false if not.</returns>
        public bool IsValidIndex(int columnIndex, int rowIndex) =>
            columnIndex >= 0 && columnIndex < this.CellsOfSpreadsheet.GetLength(0) &&
            rowIndex >= 0 && rowIndex < this.CellsOfSpreadsheet.GetLength(1);

        /// <summary>
        /// Try get the cell at the specified location.
        /// </summary>
        /// <param name="columnIndex">The column at which the cell is at.</param>
        /// <param name="rowIndex">The row at which the cell is at.</param>
        /// <param name="cell">The desired cell, returns default if not obtained.</param>
        /// <returns>Returns true is desired cell is obtained, false if not.</returns>
        public bool TryGetCell(int columnIndex, int rowIndex, out Cell? cell)
        {
            cell = default;
            if (this.IsValidIndex(columnIndex, rowIndex))
            {
                cell = this[columnIndex, rowIndex];
                return true;
            }

            return false;
        }

        /// <summary>
        /// Indexer to pass back a Spreadsheet Cell.
        /// </summary>
        /// <param name="columnIndex">The column of the location of the cell.</param>
        /// <param name="rowIndex">The row of the location of the cell.</param>
        /// <returns>A Cell.</returns>
        public Cell this[int columnIndex, int rowIndex] => this.CellsOfSpreadsheet[columnIndex, rowIndex];

        /// <summary>
        /// Gets number of columns in spreadsheet.
        /// </summary>
        public int ColumnCount => this.CellsOfSpreadsheet.GetLength(0);

        /// <summary>
        /// Gets number of rows in spreadsheet.
        /// </summary>
        public int RowCount => this.CellsOfSpreadsheet.GetLength(1);

        /// <summary>
        /// Allows undo of a command.
        /// </summary>
        public void Undo()
        {
            if (this.UndoStack.Count > 0)
            {
                Command command = this.UndoStack.Pop();
                List<Cell> ClonedCellsForRedo = new();
                foreach (SpreadsheetCell cell in command.ChangedCells.Cast<SpreadsheetCell>())
                {
                    ClonedCellsForRedo.Add(this.CellsOfSpreadsheet[cell.ColumnIndex, cell.RowIndex].Clone());
                    this.CellsOfSpreadsheet[cell.ColumnIndex, cell.RowIndex]
                        .BeforePropertyChanged -= this.BeforeCellPropertyChangedEventHandler;
                    this.CellsOfSpreadsheet[cell.ColumnIndex, cell.RowIndex].Text = cell.Text;
                    this.CellsOfSpreadsheet[cell.ColumnIndex, cell.RowIndex].BackgroundColor = cell.BackgroundColor;
                    this.CellsOfSpreadsheet[cell.ColumnIndex, cell.RowIndex].ErrorMessage = cell.ErrorMessage;
                    this.CellsOfSpreadsheet[cell.ColumnIndex, cell.RowIndex]
                        .BeforePropertyChanged += this.BeforeCellPropertyChangedEventHandler;
                }

                Command redoCommand = new(command.Description, ClonedCellsForRedo.ToArray());
                this.RedoStack.Push(redoCommand);
            }
        }

        /// <summary>
        /// Allows redo of a command.
        /// </summary>
        public void Redo()
        {
            if (this.RedoStack.Count > 0)
            {
                Command command = this.RedoStack.Pop();
                foreach (SpreadsheetCell cell in command.ChangedCells.Cast<SpreadsheetCell>())
                {
                    this.CellsOfSpreadsheet[cell.ColumnIndex, cell.RowIndex]
                        .BeforePropertyChanged -= this.BeforeCellPropertyChangedEventHandler;
                    this.CellsOfSpreadsheet[cell.ColumnIndex, cell.RowIndex].Text = cell.Text;
                    this.CellsOfSpreadsheet[cell.ColumnIndex, cell.RowIndex].BackgroundColor = cell.BackgroundColor;
                    this.CellsOfSpreadsheet[cell.ColumnIndex, cell.RowIndex].ErrorMessage = cell.ErrorMessage;
                    this.CellsOfSpreadsheet[cell.ColumnIndex, cell.RowIndex]
                        .BeforePropertyChanged += this.BeforeCellPropertyChangedEventHandler;
                }
            }
        }
    }
}

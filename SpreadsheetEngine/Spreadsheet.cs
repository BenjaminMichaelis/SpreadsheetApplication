// <copyright file="Spreadsheet.cs" company="Benjamin Michaelis">
// Copyright (c) Benjamin Michaelis. ID: 11620581. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace SpreadsheetEngine
{
    /// <summary>
    /// Is a container of a 2D array of cells.
    /// </summary>
    public partial class Spreadsheet
    {
        /// <summary>
        /// Event handler to connect to UI level.
        /// </summary>
        public event PropertyChangedEventHandler? OnCellPropertyChanged;

        private SpreadsheetCell[,] CellsOfSpreadsheet { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Spreadsheet"/> class.
        /// </summary>
        /// <param name="rows">Number of rows.</param>
        /// <param name="columns">Number of columns.</param>
        public Spreadsheet(int rows, int columns)
        {
            this.ColumnCount = columns;
            this.RowCount = rows;
            this.CellsOfSpreadsheet = new SpreadsheetCell[rows, columns];
            for (int rowNum = 0; rowNum < rows; rowNum++)
            {
                for (int colNum = 0; colNum < columns; colNum++)
                {
                    this.CellsOfSpreadsheet[rowNum, colNum] = new SpreadsheetCell(rowNum, colNum, this);
#pragma warning disable CS8622 // Nullability of reference types in type of parameter doesn't match the target delegate (possibly because of nullability attributes).
                    this.CellsOfSpreadsheet[rowNum, colNum].PropertyChanged += this.CellPropertyChanged;
#pragma warning disable CS8622 // Nullability of reference types in type of parameter doesn't match the target delegate (possibly because of nullability attributes).

                }
            }
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

                SpreadsheetCell temp = (SpreadsheetCell)this[randomRow!, randomCol!];
                temp!.Text = "Hello World";
                this.CellsOfSpreadsheet[randomRow, randomCol] = temp;
            }

            for (int i = 0; i < 50; i++)
            {
                this.CellsOfSpreadsheet[i, 1].Text = $"This is SpreadsheetCell B{(i + 1).ToString()}";
            }
        }

        /// <summary>
        /// Notifies for  when any property for any cell in the worksheet has changed.
        /// </summary>
        /// <param name="sender">Object that called object.</param>
        /// <param name="e">The property changed arg.</param>
        public void CellPropertyChanged(object sender, PropertyChangedEventArgs e)
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
                return (SpreadsheetCell)this[rowLocation - 1, columnLocation - 1];
            }
        }

        /// <summary>
        /// Gets or sets returns number of columns in spreadsheet.
        /// </summary>
        public int ColumnCount { get; set; }

        /// <summary>
        /// Gets or sets returns number of rows in spreadsheet.
        /// </summary>
        public int RowCount { get; set; }

        /// <summary>
        /// Indexer to pass back a Spreadsheet Cell.
        /// </summary>
        /// <param name="rowIndex">The row of the location of the cell.</param>
        /// <param name="columnIndex">The column of the location of the cell.</param>
        /// <returns>A Cell.</returns>
        public Cell this[int rowIndex, int columnIndex] => this.CellsOfSpreadsheet[rowIndex, columnIndex];
    }
}

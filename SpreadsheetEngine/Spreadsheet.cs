// <copyright file="Spreadsheet.cs" company="Benjamin Michaelis">
// Copyright (c) Benjamin Michaelis. ID: 11620581. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadsheetEngine
{
    /// <summary>
    /// Is a container of a 2D array of cells.
    /// </summary>
    public class Spreadsheet
    {
        private class Cell : SpreadsheetCell
        {
            private Cell(int rowIndex, int columnIndex)
                : base(rowIndex, columnIndex)
            {
            }

            public void SetCellValue(string value)
            {
                // this. or base.
                this._value = value;
            }
        }

        private Cell[,] _cellsOfSpreadsheet;

        /// <summary>
        /// Initializes a new instance of the <see cref="Spreadsheet"/> class.
        /// </summary>
        /// <param name="rows">Number of rows.</param>
        /// <param name="columns">Number of columns.</param>
        public Spreadsheet(int rows, int columns)
        {
            this.ColumnCount = columns;
            this.RowCount = rows;
            this._cellsOfSpreadsheet = new Cell[rows, columns];

            // this._cellsOfSpreadsheet[rownum, colnum].PropertyChanged += this.CellPropertyChanged;
        }

        /// <summary>
        /// Notifies for  when any property for any cell in the worksheet has changed.
        /// </summary>
        /// <param name="sender">Object that called object.</param>
        /// <param name="e">The property changed arg.</param>
        public void CellPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Cell? evaluatingCell = sender as Cell;
            if (evaluatingCell != null)
            {
                if (e.PropertyName == "Text")
                {
                    if (!string.IsNullOrEmpty(evaluatingCell.Text))
                    {
                        if (evaluatingCell.Text.StartsWith("="))
                        {
                            Cell? targetCell = this.GetCell(evaluatingCell.Text.Substring(1).ToString());
                            if (targetCell != null)
                            {
                                evaluatingCell.SetCellValue(targetCell.Value);
                            }
                        }
                        else
                        {
                            evaluatingCell.SetCellValue(evaluatingCell.Text);
                        }

                        this.OnCellPropertyChanged.Invoke(sender, e);
                    }
                }
            }
        }

        /// <summary>
        /// Event handler to connect to UI level.
        /// </summary>
        public event PropertyChangedEventHandler OnCellPropertyChanged = (sender, e) => { };

        /// <summary>
        /// Converts the first letters in a string to numbers.
        /// </summary>
        /// <param name="cellName">The name of the cell (ex: AA33).</param>
        /// <returns>Int of Cell.</returns>
        public int ColumnLetterToInt(string cellName)
        {
            string columnLetters = string.Concat(cellName.TakeWhile(char.IsLetter));
            int columnLocation = columnLetters.ToCharArray().Select(c => c - 'A' + 1).Reverse().Select((v, i) => v * (int)Math.Pow(26, i)).Sum();
            return columnLocation;
        }

        /// <summary>
        /// Get the cell from the cell name.
        /// </summary>
        /// <param name="cellName">Letter/number combo of cell.</param>
        /// <returns>Returns cell in spreadsheet that matches the index of the cell.</returns>
        private Cell? GetCell(string cellName)
        {
            int columnLocation = this.ColumnLetterToInt(cellName);
            string rowLocationString = string.Join(null, System.Text.RegularExpressions.Regex.Split(cellName, "[^\\d]"));
            int rowLocation = int.Parse(rowLocationString);
            return this.GetCell(rowLocation, columnLocation);
        }

        /// <summary>
        /// GetCell function that takes a row and column index and returns the cell at that location or null if there is no such cell.
        /// </summary>
        /// <param name="rowIndex">Pass in the row of the cell you want to access.</param>
        /// <param name="colIndex">Pass in the column of the cell you want to access.</param>
        /// <returns>Returns a Cell of Cell.</returns>
        private Cell? GetCell(int rowIndex, int colIndex)
        {
            return this._cellsOfSpreadsheet[rowIndex, colIndex];
        }

        /// <summary>
        /// Gets or sets returns number of columns in spreadsheet.
        /// </summary>
        public int ColumnCount { get; set; }

        /// <summary>
        /// Gets or sets returns number of rows in spreadsheet.
        /// </summary>
        public int RowCount { get; set; }
    }
}

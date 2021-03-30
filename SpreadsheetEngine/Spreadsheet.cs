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
            /// <summary>
            /// Initializes a new instance of the <see cref="Cell"/> class.
            /// </summary>
            /// <param name="rowIndex">Row index of cell.</param>
            /// <param name="columnIndex">Column index of cell.</param>
            public Cell(int rowIndex, int columnIndex)
                : base(rowIndex, columnIndex)
            {
            }

            public void SetCellValue(string value)
            {
                // this. or base.
                this._value = value;
            }
        }

        /// <summary>
        /// Event handler to connect to UI level.
        /// </summary>
        public event PropertyChangedEventHandler? OnCellPropertyChanged;

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
            for (int rowNum = 0; rowNum < rows; rowNum++)
            {
                for (int colNum = 0; colNum < columns; colNum++)
                {
                    this._cellsOfSpreadsheet[rowNum, colNum] = new Cell(rowNum, colNum);
#pragma warning disable CS8622 // Nullability of reference types in type of parameter doesn't match the target delegate (possibly because of nullability attributes).
                    this._cellsOfSpreadsheet[rowNum, colNum].PropertyChanged += this.CellPropertyChanged;
#pragma warning disable CS8622 // Nullability of reference types in type of parameter doesn't match the target delegate (possibly because of nullability attributes).

                }
            }

            // this._cellsOfSpreadsheet[rownum, colnum].PropertyChanged += this.CellPropertyChanged;
        }

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
        /// Runs a demo of the code with 50 cells displaying "Hello World".
        /// </summary>
        public void Demo()
        {
            int randomRow = 0, randomCol = 0;
            Random random = new Random();

            for (int i = 0; i < 50; i++)
            {
                randomCol = random.Next(0, 25);
                randomRow = random.Next(0, 49);

                Cell temp = this.GetCell(randomRow!, randomCol!);
                temp!.Text = "Hello World";
                this._cellsOfSpreadsheet[randomRow, randomCol] = temp;
            }

            for (int i = 0; i < 50; i++)
            {
                this._cellsOfSpreadsheet[i, 1].Text = $"This is Cell B{(i+1).ToString()}";
            }
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
                if (e.PropertyName == nameof(SpreadsheetCell.Text))
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

                        this.OnCellPropertyChanged!.Invoke(sender, e);
                    }
                }
            }
        }

        /// <summary>
        /// GetCell function that takes a row and column index and returns the cell at that location or null if there is no such cell.
        /// </summary>
        /// <param name="rowIndex">Pass in the row of the cell you want to access.</param>
        /// <param name="colIndex">Pass in the column of the cell you want to access.</param>
        /// <returns>Returns a Cell of Cell.</returns>
        public string? GetCellText(int rowIndex, int colIndex)
        {
            if ((rowIndex <= this.RowCount && rowIndex >= 0) && colIndex <= this.ColumnCount && colIndex >= 0)
            {
                return this._cellsOfSpreadsheet[rowIndex, colIndex].Text;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// GetCell function that takes a row and column index and returns the cell at that location or null if there is no such cell.
        /// </summary>
        /// <param name="rowIndex">Pass in the row of the cell you want to access.</param>
        /// <param name="colIndex">Pass in the column of the cell you want to access.</param>
        /// <returns>Returns a Cell of Cell.</returns>
        public string? GetCellValue(int rowIndex, int colIndex)
        {
            if ((rowIndex <= this.RowCount && rowIndex >= 0) && colIndex <= this.ColumnCount && colIndex >= 0)
            {
                return this._cellsOfSpreadsheet[rowIndex, colIndex].Value;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// GetCell function that takes a row and column index and returns the text at that location or null if there is no such cell.
        /// </summary>
        /// <param name="rowIndex">Pass in the row of the cell you want to access.</param>
        /// <param name="colIndex">Pass in the column of the cell you want to access.</param>
        /// <returns>Returns a string of the cell.</returns>
        private Cell? GetCell(int rowIndex, int colIndex)
        {
            if ((rowIndex <= this.RowCount && rowIndex >= 0) && colIndex <= this.ColumnCount && colIndex >= 0)
            {
                return this._cellsOfSpreadsheet[rowIndex, colIndex];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Get the cell from the cell name.
        /// </summary>
        /// <param name="cellName">Letter/number combo of cell.</param>
        /// <returns>Returns cell in spreadsheet that matches the index of the cell.</returns>
        private Cell? GetCell(string cellName)
        {
            int columnLocation = ColumnLetterToInt(cellName);
            string rowLocationString = string.Join(null, System.Text.RegularExpressions.Regex.Split(cellName, "[^\\d]"));
            int rowLocation = int.Parse(rowLocationString);
            return this.GetCell(rowLocation, columnLocation);
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

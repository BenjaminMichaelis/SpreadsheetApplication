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
        private SpreadsheetCell[,] _cellsOfSpreadsheet;

        /// <summary>
        /// Initializes a new instance of the <see cref="Spreadsheet"/> class.
        /// </summary>
        /// <param name="rows">Number of rows.</param>
        /// <param name="columns">Number of columns.</param>
        public Spreadsheet(int rows, int columns)
        {
            this.ColumnCount = columns;
            this.RowCount = rows;
            this._cellsOfSpreadsheet = new SpreadsheetCell[rows, columns];
        }

        /// <summary>
        /// Notifies for  when any property for any cell in the worksheet has changed.
        /// </summary>
        /// <param name="sender">Object that called object.</param>
        /// <param name="e">The property changed arg.</param>
        public void CellPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            SpreadsheetCell? evaluatingCell = sender as SpreadsheetCell;
            if (string.IsNullOrEmpty(evaluatingCell.Text))
            {
                if (evaluatingCell.Text.StartsWith("="))
                {
                    
                }
            }
        }

        /// <summary>
        /// GetCell function that takes a row and column index and returns the cell at that location or null if there is no such cell.
        /// </summary>
        /// <param name="rowIndex">Pass in the row of the cell you want to access.</param>
        /// <param name="colIndex">Pass in the column of the cell you want to access.</param>
        /// <returns>Returns a Cell of SpreadsheetCell.</returns>
        public SpreadsheetCell? GetCell(int rowIndex, int colIndex)
        {
            try
            {
                return this._cellsOfSpreadsheet[rowIndex, colIndex];
            }
            catch
            {
                return null;
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
    }
}

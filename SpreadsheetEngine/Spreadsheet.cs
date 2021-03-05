// <copyright file="Spreadsheet.cs" company="Benjamin Michaelis">
// Copyright (c) Benjamin Michaelis. ID: 11620581. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
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
        /// Initializes spreadsheet with rows and columns of cells.
        /// </summary>
        /// <param name="rows">Number of rows.</param>
        /// <param name="columns">Number of columns.</param>
        public Spreadsheet(int rows, int columns)
        {
            this._cellsOfSpreadsheet = new SpreadsheetCell[rows, columns];
        }
    }
}

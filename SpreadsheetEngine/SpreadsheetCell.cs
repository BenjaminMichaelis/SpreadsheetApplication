// <copyright file="SpreadsheetCell.cs" company="Benjamin Michaelis">
// Copyright (c) Benjamin Michaelis. ID: 11620581. All rights reserved.
// </copyright>

using System;

namespace SpreadsheetEngine
{
    /// <summary>
    /// Represents one cell in the worksheet.
    /// </summary>
    public abstract class SpreadsheetCell
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SpreadsheetCell"/> class.
        /// </summary>
        /// <param name="columnIndex">sets the readonly columnIndex.</param>
        /// <param name="rowIndex">sets the readonly rowIndex.</param>
        public SpreadsheetCell(int rowIndex, int columnIndex)
        {
            this._rowIndex = rowIndex;
            this._columnIndex = columnIndex;
        }

        private readonly int _rowIndex;

        /// <summary>
        /// Gets (read-only) row index.
        /// </summary>
        public int RowIndex
        {
            get { return this._rowIndex; }
        }

        private readonly int _columnIndex;

        /// <summary>
        /// Gets (read-only) column index.
        /// </summary>
        public int ColumnIndex
        {
            get { return this._columnIndex; }
        }
    }
}

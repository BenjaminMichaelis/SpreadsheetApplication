// <copyright file="SpreadsheetCell.cs" company="Benjamin Michaelis">
// Copyright (c) Benjamin Michaelis. ID: 11620581. All rights reserved.
// </copyright>

using System;
using System.ComponentModel;
using System.Data;

namespace SpreadsheetEngine
{
    /// <summary>
    /// Represents one cell in the worksheet.
    /// </summary>
    public abstract class SpreadsheetCell : INotifyPropertyChanged
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SpreadsheetCell"/> class.
        /// </summary>
        /// <param name="columnIndex">int for the readonly columnIndex.</param>
        /// <param name="rowIndex">int for the readonly rowIndex.</param>
        public SpreadsheetCell(int rowIndex, int columnIndex)
        {
            this.RowIndex = rowIndex;
            this.ColumnIndex = columnIndex;
            this.Text = string.Empty;
        }

        /// <summary>
        /// Gets (read-only) row index.
        /// </summary>
        public int RowIndex { get; }

        /// <summary>
        /// Gets (read-only) column index.
        /// </summary>
        public int ColumnIndex { get; }

        /// <summary>
        /// Throw Property changed if text in cell is changed.
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// stores protected string text.
        /// </summary>
#pragma warning disable SA1401 // Fields should be private - We want it private in this case.
        protected string? _text;
#pragma warning restore SA1401 // Fields should be private

        /// <summary>
        /// Gets or Sets Text that's typed into the cell.
        /// </summary>
        public string Text
        {
            get
            {
                return this._text is { } result ? result : string.Empty;
            }

            set
            {
                if (this._text != value)
                {
                    this._text = value;
                    this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.Text)));
                }
            }
        }

        /// <summary>
        /// Gets the Cell name by converting the column to a letter and adding on the row number, and returning as a string.
        /// </summary>
        public string IndexName
        {
            get
            {
                return this.ColumnIntToLetter(this.ColumnIndex) + (this.RowIndex + 1).ToString();
            }
        }

        /// <summary>
        /// stores protected value string.
        /// </summary>
#pragma warning disable SA1401 // Fields should be private
        protected string? _value;
#pragma warning restore SA1401 // Fields should be private

        /// <summary>
        /// Gets Value which is text if not set or function if it is.
        /// </summary>
        public string Value
        {
            get
            {
                return this._value is { } result ? result : string.Empty;
            }
        }

        /// <summary>
        /// Converts an associated Letter to int value for Index Name.
        /// </summary>
        /// <param name="columnLetter">Letter to be switched to int.</param>
        /// <returns>Returns an int of the column int.</returns>
        public int ColumnLetterToInt(string columnLetter)
        {
            int sum = 0;
            columnLetter = columnLetter.ToUpperInvariant(); // Convert to uppercase if not already.
            foreach (char c in columnLetter)
            {
                sum *= 26;
                sum += c - 'A' + 1;
            }

            return sum;
        }

        /// <summary>
        /// Converts an Int to associated Letter for Index Name.
        /// </summary>
        /// <param name="index">Int to be switched to letters.</param>
        /// <returns>Returns string of letters for index.</returns>
        public string ColumnIntToLetter(int index)
        {
            int dividend = index;
            string columnName = string.Empty;
            int modulo;
            while (dividend > 0)
            {
                modulo = (dividend - 1) % 26;
                columnName = Convert.ToChar(65 + modulo).ToString() + columnName;
                dividend = (int)((dividend - modulo) / 26);
            }

            return columnName;
        }
    }
}

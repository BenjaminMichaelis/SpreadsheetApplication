// <copyright file="Cell.cs" company="Benjamin Michaelis">
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
    public abstract class Cell : INotifyPropertyChanged
    {
        /// <summary>
        /// Error Message if circular reference occurs.
        /// </summary>
        public const string CellErrorMessage = "#error";

        /// <summary>
        /// Initializes a new instance of the <see cref="Cell"/> class.
        /// </summary>
        /// <param name="columnIndex">int for the readonly columnIndex.</param>
        /// <param name="rowIndex">int for the readonly rowIndex.</param>
        protected Cell(int columnIndex, int rowIndex)
        {
            this.RowIndex = rowIndex;
            this.ColumnIndex = columnIndex;
            this.Text = string.Empty;
            this.ErrorMessage = null;
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
        /// Stores protected string text.
        /// </summary>
#pragma warning disable SA1401 // Fields should be private - We want it private in this case.
        protected string? _text;
#pragma warning restore SA1401 // Fields should be private

        /// <summary>
        /// Gets or Sets Text that's typed into the cell.
        /// </summary>
        public string Text
        {
            get => this._text is { } result ? result : string.Empty;

            set
            {
                if (this._text == value)
                {
                    return;
                }

                this._text = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.Text)));
            }
        }

        /// <summary>
        /// Gets the Cell name by converting the column to a letter and adding on the row number, and returning as a string.
        /// </summary>
        public string IndexName => this.ColumnIntToLetter(this.ColumnIndex + 1) + (this.RowIndex + 1).ToString();

        private string? _internalValue;

        /// <summary>
        /// Gets or sets the error message to the cell.
        /// </summary>
        public string? ErrorMessage { get; set; }

        /// <summary>
        /// Gets a value indicating whether gets bool; True if is an error message, false if not.
        /// </summary>
        public bool IsErrored => !string.IsNullOrWhiteSpace(this.ErrorMessage);

        /// <summary>
        /// Stores cell background color.
        /// </summary>
#pragma warning disable SA1401 // Fields should be private - We want it private in this case.
        protected uint _backgroundColor = 0xFFFFFFFF;
#pragma warning restore SA1401 // Fields should be private - We want it private in this case.

        /// <summary>
        /// Gets or sets cell background color.
        /// </summary>
        public uint BackgroundColor
        {
            get => this._backgroundColor;
            set
            {
                if (this._backgroundColor == value)
                {
                    return;
                }

                this._backgroundColor = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.BackgroundColor)));
            }
        }

        /// <summary>
        /// Gets Value which is text if not set or function if it is.
        /// </summary>
        public string Value
        {
            get
            {
                if (this.IsErrored)
                {
                    return this.ErrorMessage ?? throw new InvalidOperationException();
                }
                else
                {
                    return this.InternalValue is { } result ? result : string.Empty;
                }
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
            while (dividend > 0)
            {
                int modulo = (dividend - 1) % 26;
                columnName = Convert.ToChar(65 + modulo).ToString() + columnName;
                dividend = (int)((dividend - modulo) / 26);
            }

            return columnName;
        }

        /// <summary>
        /// Gets or sets the internal value of the cell that can invoke an event.
        /// </summary>
        protected string? InternalValue
        {
            get => this._internalValue;
            set
            {
                if (value != this._internalValue)
                {
                    this._internalValue = value;
                    this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.Value)));
                }
            }
        }

        /// <summary>
        /// Allows for cell info to be displayed in debug watch window.
        /// </summary>
        /// <returns>Returns overriden string with specified information.</returns>
        public override string ToString() => $"{this.IndexName}: Text='{this.Text}', Value='{this.Value}'";
    }
}

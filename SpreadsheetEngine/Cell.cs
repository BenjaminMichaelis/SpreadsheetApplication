// <copyright file="Cell.cs" company="Benjamin Michaelis">
// Copyright (c) Benjamin Michaelis. All rights reserved.
// </copyright>

using System;
using System.ComponentModel;
using System.Data;
using System.Linq;

namespace SpreadsheetEngine
{
    /// <summary>
    /// Represents one cell in the worksheet.
    /// </summary>
    public abstract class Cell : INotifyPropertyChanged
    {
        /// <summary>
        /// Clones the current cell.
        /// </summary>
        /// <returns>Returns a copy of the current cell.</returns>
        public abstract Cell Clone();

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
        /// Event handler to hold the property changed if property in cell is changed.
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Event handler to hold the cell before it gets changed.
        /// </summary>
        public event EventHandler<BeforeCellChangedEventArgs>? BeforePropertyChanged;

        /// <summary>
        /// Gets or Sets Text that's typed into the cell.
        /// </summary>
        public string Text
        {
            get => this.InternalText is { } result ? result : string.Empty;

            set
            {
                if (this.InternalText == value)
                {
                    return;
                }

                this.BeforePropertyChanged?.Invoke(this, new BeforeCellChangedEventArgs(
                    $"Undo change of cell {nameof(this.Text)}",
                    this.Clone()));
                this.InternalText = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.Text)));
            }
        }

        /// <summary>
        /// Gets the Cell name by converting the column to a letter and adding on the row number, and returning as a string.
        /// </summary>
        public string IndexName => this.ColumnIntToLetter(this.ColumnIndex + 1) + (this.RowIndex + 1).ToString();

        private string? _internalValue;

        private string? _errorMessage;

        /// <summary>
        /// Gets or sets the error message to the cell.
        /// </summary>
        public string? ErrorMessage
        {
            get => this._errorMessage;
            set
            {
                this._errorMessage = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.ErrorMessage)));
            }
        }

        /// <summary>
        /// Gets a value indicating whether gets bool; True if is an error message, false if not.
        /// </summary>
        public bool IsErrored => !string.IsNullOrWhiteSpace(this.ErrorMessage);

        /// <summary>
        /// Gets or sets cell background color.
        /// </summary>
        public uint BackgroundColor
        {
            get => this.InternalBackgroundColor;
            set
            {
                if (this.InternalBackgroundColor == value)
                {
                    return;
                }

                this.BeforePropertyChanged?.Invoke(this, new BeforeCellChangedEventArgs(
                    $"Undo change of cell {nameof(this.BackgroundColor)}",
                    this.Clone()));
                this.InternalBackgroundColor = value;
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
        /// <param name="cellName">Letter to be switched to int.</param>
        /// <returns>Returns an int of the column int.</returns>
        public static int ColumnLetterToInt(string cellName)
        {
            int columnLocation = string.Concat(cellName.TakeWhile(char.IsLetter)) // Get the letters from the cell name.
                .ToUpperInvariant() // Make all the letters in the cell name uppercase.
                .ToCharArray().Select(c => c - 'A' + 1).Reverse().Select((v, i) => v * (int)Math.Pow(26, i)).Sum(); // Calculate the number the letter(s) associate too.
            return columnLocation;
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
        /// Gets or sets protected string text.
        /// </summary>
        protected string? InternalText { get; set; }

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
        /// Gets or sets cell background color.
        /// </summary>
        protected uint InternalBackgroundColor { get; set; } = 0xFFFFFFFF;

        /// <summary>
        /// Allows for cell info to be displayed in debug watch window.
        /// </summary>
        /// <returns>Returns overriden string with specified information.</returns>
        public override string ToString() => $"{this.IndexName}: Text='{this.Text}', Value='{this.Value}'";
    }
}

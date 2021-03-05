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
            this.Value = this.Text;
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

        private string? _text;

        /// <summary>
        /// Gets or Sets Text thats typed into the cell.
        /// </summary>
        protected string Text
        {
            get
            {
                if (string.IsNullOrEmpty(this._text))
                {
                    return string.Empty;
                }
                else
                {
                    return this._text;
                }
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

        private string? _value;

        /// <summary>
        /// Gets or sets Value which is text if not set or function if it is.
        /// </summary>
        protected string Value
        {
            get
            {
                if (string.IsNullOrEmpty(this._value))
                {
                    return string.Empty;
                }
                else
                {
                    return this._value;
                }
            }

            set
            {
                // // evaluated value of cell
                // if(this.Text[0] == '=')
                // {
                //     DataTable dt = new DataTable();
                //     int answer = (int)dt.Compute(this.Text, string.Empty);
                //     this._value = answer.ToString();
                // }
                if (this._value != value)
                {
                    this._value = this.Text;
                }
            }
        }
    }
}

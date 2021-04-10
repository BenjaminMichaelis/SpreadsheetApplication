// <copyright file="RestoreHistory.cs" company="Benjamin Michaelis">
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
    /// Logic to restore history forward or backwards.
    /// </summary>
    public class RestoreHistory : IHistoryCommand
    {
        public static RestoreHistory CreateInstance(Cell cell, string propertyName, uint? newColor = null, string? newText = null)
        {
            return new RestoreHistory(cell, propertyName, newColor, newText);
        }

        private Cell? cell;
        private string? propertyName;
        private string? text;
        private uint color;

        /// <summary>
        /// Initializes a new instance of the <see cref="RestoreHistory"/> class.
        /// </summary>
        /// <param name="cell">The cell that is being saved.</param>
        /// <param name="propertyName">The property name that was changed.</param>
        /// <param name="newColor">The new color if it was changed.</param>
        /// <param name="newText">The new text if it was changed.</param>
        private RestoreHistory(Cell cell, string propertyName, uint? newColor = null, string? newText = null)
        {
            if (propertyName == nameof(Cell.BackgroundColor) && newColor != null)
            {
                this.cell = cell;
                this.color = (uint)newColor;
            }

            if (propertyName == nameof(Cell.BackgroundColor))
            {
                this.cell = cell;
                this.color = cell.BackgroundColor;
            }

            switch (propertyName)
            {
                case nameof(Cell.BackgroundColor) when newText != null:
                    this.cell = cell;
                    this.text = newText;
                    break;
                case nameof(Cell.Text):
                    this.cell = cell;
                    this.text = cell.Text;
                    break;
            }
        }

        /// <summary>
        /// Executes a history command.
        /// </summary>
        /// <param name="spreadsheet">The spreadsheet class.</param>
        /// <returns>Returns a RestoreHistory command.</returns>
        public IHistoryCommand Execute(Spreadsheet spreadsheet)
        {
            switch (this.propertyName)
            {
                case nameof(Cell.Text):
                {
                    string currentText = spreadsheet[this.cell.RowIndex, this.cell.ColumnIndex].Text;
                    spreadsheet[this.cell.RowIndex, this.cell.ColumnIndex].Text = this.text;
                    return CreateInstance(spreadsheet[this.cell.RowIndex, this.cell.ColumnIndex], nameof(Cell.Text), newColor: null,currentText);
                }

                case nameof(Cell.BackgroundColor):
                {
                    uint currentColor = this.cell.BackgroundColor;
                    this.cell.BackgroundColor = this.color;
                    return CreateInstance(this.cell, nameof(this.cell.BackgroundColor), currentColor);
                }

                default:
                    return CreateInstance(this.cell, "Other");
            }
        }
    }
}

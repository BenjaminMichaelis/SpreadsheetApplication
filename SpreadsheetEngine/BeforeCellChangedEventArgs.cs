// <copyright file="BeforeCellChangedEventArgs.cs" company="Benjamin Michaelis">
// Copyright (c) Benjamin Michaelis. ID: 11620581. All rights reserved.
// </copyright>

using System;

namespace SpreadsheetEngine
{
    /// <summary>
    /// Event argument to store cell before it changes.
    /// </summary>
    public class BeforeCellChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets the cells before they get changed.
        /// </summary>
        public Cell[] CellsBeforeChange { get; set; }

        /// <summary>
        /// Gets or sets the description of what in the cells is changing.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BeforeCellChangedEventArgs"/> class.
        /// </summary>
        /// <param name="description">Description of what in the cells is changing.</param>
        /// <param name="cellsBeforeChange">The cells before they get changed.</param>
        public BeforeCellChangedEventArgs(string description, params Cell[] cellsBeforeChange)
        {
            this.CellsBeforeChange = cellsBeforeChange;
            this.Description = description;
        }
    }
}

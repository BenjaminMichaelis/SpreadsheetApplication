// <copyright file="Command.cs" company="Benjamin Michaelis">
// Copyright (c) Benjamin Michaelis. All rights reserved.
// </copyright>

namespace SpreadsheetEngine
{
    /// <summary>
    /// Class containing the cells changes and what changes.
    /// </summary>
    public class Command
    {
        /// <summary>
        /// Gets the cells that got changed.
        /// </summary>
        public Cell[] ChangedCells { get; }

        /// <summary>
        /// Gets description of what changed.
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Command"/> class.
        /// </summary>
        /// <param name="description">Description of what changed.</param>
        /// <param name="changedCells">The cells that got changed.</param>
        public Command(string description, params Cell[] changedCells)
        {
            this.ChangedCells = changedCells;
            this.Description = description;
        }
    }
}

// <copyright file="BeforeCellChangedEventArgs.cs" company="Benjamin Michaelis">
// Copyright (c) Benjamin Michaelis. ID: 11620581. All rights reserved.
// </copyright>

using System;

namespace SpreadsheetEngine
{
    public class BeforeCellChangedEventArgs : EventArgs
    {
        public Cell CellBeforeChange { get; set; }

        public BeforeCellChangedEventArgs(Cell cellBeforeChange)
        {
            CellBeforeChange = cellBeforeChange;
        }
    }
}

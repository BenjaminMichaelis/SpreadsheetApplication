// <copyright file="CellTests.cs" company="Benjamin Michaelis">
// Copyright (c) Benjamin Michaelis. ID: 11620581. All rights reserved.
// </copyright>

using System;
using Xunit;

namespace SpreadsheetEngine.Tests
{
    /// <summary>
    /// Test methods in the spreadsheetClass.
    /// </summary>
    public class CellTests
    {
        /// <summary>
        /// Constructs class for abstract Class Cell.
        /// </summary>
        public class SpreadsheetCellTests : Cell
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="SpreadsheetCellTests"/> class.
            /// Constructs object for abstract Class Cell.
            /// </summary>
            /// <param name="rowIndex">The row index of cell.</param>
            /// <param name="columnIndex">The column index of the cell.</param>
            public SpreadsheetCellTests(int rowIndex, int columnIndex)
                : base(columnIndex, rowIndex)
            {
            }

            /// <summary>
            /// Method that would need to be overwritten.
            /// </summary>
            /// <returns>Clone of the current cell.</returns>
            public override Cell Clone()
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Test that when given the int 1 for a column number it returns A.
        /// </summary>
        [Fact]
        public void ColumnNumberToLetter_A_return1()
        {
            SpreadsheetCellTests sut = new(1, 1);
            Assert.Equal("A", sut.ColumnIntToLetter(1));
        }

        /// <summary>
        /// Test that when given the int 26 for a column number it returns Z.
        /// </summary>
        [Fact]
        public void ColumnNumberToLetter_26_returnZ()
        {
            SpreadsheetCellTests sut = new(1, 1);
            Assert.Equal("Z", sut.ColumnIntToLetter(26));
        }

        /// <summary>
        /// Test that when given the int 34 for a column number it returns AH.
        /// </summary>
        [Fact]
        public void ColumnNumberToLetter_34_returnAH()
        {
            SpreadsheetCellTests sut = new(1, 1);
            Assert.Equal("AH", sut.ColumnIntToLetter(34));
        }

        /// <summary>
        /// Test that when given the int 34 for a column number it returns AH.
        /// </summary>
        [Fact]
        public void ColumnNumberToLetter_16384_returnXFD()
        {
            SpreadsheetCellTests sut = new(1, 1);
            Assert.Equal("XFD", sut.ColumnIntToLetter(16384));
        }

        /// <summary>
        /// Test that when given the int 34 for a column number it returns AH.
        /// </summary>
        [Fact]
        public void ColumnLetterToNumber_XFD_return16384()
        {
            SpreadsheetCellTests sut = new(1, 1);
            Assert.Equal(16384, sut.ColumnLetterToInt("XFD"));
        }

        /// <summary>
        /// Test that when given the int 34 for a column number it returns AH.
        /// </summary>
        [Fact]
        public void ColumnLetterToNumber_Z_return26()
        {
            SpreadsheetCellTests sut = new(1, 1);
            Assert.Equal(26, sut.ColumnLetterToInt("Z"));
        }
    }
}

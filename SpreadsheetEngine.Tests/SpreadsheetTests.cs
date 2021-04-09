// <copyright file="SpreadsheetTests.cs" company="Benjamin Michaelis">
// Copyright (c) Benjamin Michaelis. ID: 11620581. All rights reserved.
// </copyright>

using System;
using Xunit;

namespace SpreadsheetEngine.Tests
{
    /// <summary>
    /// Test methods in the spreadsheetClass.
    /// </summary>
    public class SpreadsheetTests
    {
        /// <summary>
        /// Test that when given the letter A for a column number, it returns 1.
        /// </summary>
        [Fact]
        public void ColumnLetterToNumber_A_return1()
        {
            Spreadsheet sut = new(1, 1);
            Assert.Equal(1, sut.ColumnLetterToInt("A"));
        }

        /// <summary>
        /// Test that when given the letter AH for a column number, it returns 34.
        /// </summary>
        [Fact]
        public void ColumnLetterToNumber_AH_return34()
        {
            Spreadsheet sut = new(1, 1);
            Assert.Equal(34, sut.ColumnLetterToInt("AH"));
        }

        /// <summary>
        /// Test that when given the letter XFD for a column number, it returns 16384.
        /// </summary>
        [Fact]
        public void ColumnLetterToNumber_XFD_return16384()
        {
            Spreadsheet sut = new(1, 1);
            Assert.Equal(16384, sut.ColumnLetterToInt("XFD"));
        }

        /// <summary>
        /// Test references.
        /// </summary>
        [Fact]
        public void ReferenceCell()
        {
            Spreadsheet sut = new(2, 1);
            sut.SetCellText(0, 0, "10");
            sut.SetCellText(1, 0, "=A1");
            Assert.Equal("10", sut.GetCellValue(1, 0));
        }

        /// <summary>
        /// Test Circular references.
        /// </summary>
        [Fact]
        public void CircularReference()
        {
            Spreadsheet sut = new(2, 1);
            sut.SetCellText(0, 0, "=A2");
            sut.SetCellText(1, 0, "=A1");
            Assert.Equal("#error", sut.GetCellValue(0, 0));
            Assert.Equal("#error", sut.GetCellValue(1, 0));
            sut.SetCellText(0, 0, "=10");
            Assert.Equal("10", sut.GetCellValue(0, 0));
            Assert.Equal("10", sut.GetCellValue(1, 0));
        }
    }
}

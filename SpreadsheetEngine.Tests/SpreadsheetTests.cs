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
            Assert.Equal(1, sut.ColumnLetterToNumber("A"));
        }

        /// <summary>
        /// Test that when given the letter AH for a column number, it returns 34.
        /// </summary>
        [Fact]
        public void ColumnLetterToNumber_AH_return34()
        {
            Spreadsheet sut = new(1, 1);
            Assert.Equal(34, sut.ColumnLetterToNumber("AH"));
        }

        /// <summary>
        /// Test that when given the letter XFD for a column number, it returns 16384.
        /// </summary>
        [Fact]
        public void ColumnLetterToNumber_XFD_return16384()
        {
            Spreadsheet sut = new(1, 1);
            Assert.Equal(16384, sut.ColumnLetterToNumber("XFD"));
        }
    }
}

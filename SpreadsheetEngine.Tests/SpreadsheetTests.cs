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
            Assert.Equal("10", sut[1, 0].Value);
        }

        /// <summary>
        /// Test Circular references.
        /// </summary>
        [Fact]
        public void CircularReference()
        {
            Spreadsheet sut = new(2, 1);
            sut[0, 0].Text = "=A2";
            sut[1, 0].Text = "=A1";
            Assert.Equal("#error", sut[0, 0].Value);
            Assert.Equal("#error", sut[1, 0].Value);
            sut[0, 0].Text = "=10";
            Assert.Equal("10", sut[0, 0].Value);
            Assert.Equal("10", sut[1, 0].Value);
        }

        /// <summary>
        /// Test cell adding another cell reference.
        /// </summary>
        [Fact]
        public void CellAddingAnotherCell()
        {
            Spreadsheet sut = new(3, 1);
            sut[0, 0].Text = "=10";
            sut[1, 0].Text = "=11";
            sut[2, 0].Text = "=A1+A2";
            Assert.Equal("10", sut[0, 0].Value);
            Assert.Equal("11", sut[1, 0].Value);
            Assert.Equal("21", sut[2, 0].Value);
        }

        /// <summary>
        /// Test background color changing.
        /// </summary>
        [Fact]
        public void BackgroundColorChange()
        {
            Spreadsheet sut = new(2, 1);
            sut[0, 0].BackgroundColor = 0xFFFF7F50;
            sut[1, 0].BackgroundColor = 0xFF6495ED;
            Assert.Equal(0xFFFF7F50, sut[0, 0].BackgroundColor);
            Assert.Equal(0xFF6495ED, sut[1, 0].BackgroundColor);
        }

        public void UndoCommand()
        {
            Spreadsheet sut = new(5, 5);
            sut[0, 0].Text = "=88";
            sut[0, 0].Text = "=12";
            sut.Undo();
            Assert.Equal("=88", sut[0, 0].Text);
            Assert.Equal("88", sut[0, 0].Value);
        }
    }
}

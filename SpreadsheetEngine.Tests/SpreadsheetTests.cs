// <copyright file="SpreadsheetTests.cs" company="Benjamin Michaelis">
// Copyright (c) Benjamin Michaelis. ID: 11620581. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.IO;
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
            Spreadsheet sut = new(1, 2);
            sut[0, 0].Text = "10";
            sut[0, 1].Text = "=A1";
            Assert.Equal("10", sut[0, 1].Value);
        }

        /// <summary>
        /// Test Circular references.
        /// </summary>
        [Fact]
        public void CircularReference()
        {
            Spreadsheet sut = new(1, 2);
            sut[0, 0].Text = "=A2";
            sut[0, 1].Text = "=A1";
            Assert.Equal("#error", sut[0, 0].Value);
            Assert.Equal("#error", sut[0, 1].Value);
            sut[0, 0].Text = "=10";
            Assert.Equal("10", sut[0, 0].Value);
            Assert.Equal("10", sut[0, 1].Value);
        }

        /// <summary>
        /// Test Referencing Empty Cell.
        /// </summary>
        [Fact]
        public void ReferenceEmptyCell()
        {
            Spreadsheet sut = new(1, 2);
            sut[0, 0].Text = "=A2";
            Assert.Equal("0", sut[0, 0].Value);
        }

        /// <summary>
        /// Test Referencing Empty Cell Multiplied by 2.
        /// </summary>
        [Fact]
        public void ReferenceEmptyCellPerformMath()
        {
            Spreadsheet sut = new(1, 2);
            sut[0, 0].Text = "=A2*2";
            Assert.Equal("0", sut[0, 0].Value);
        }

        /// <summary>
        /// Test cell adding another cell reference.
        /// </summary>
        [Fact]
        public void CellAddingAnotherCell()
        {
            Spreadsheet sut = new(1, 3);
            sut[0, 0].Text = "=10";
            sut[0, 1].Text = "=11";
            sut[0, 2].Text = "=A1+A2";
            Assert.Equal("10", sut[0, 0].Value);
            Assert.Equal("11", sut[0, 1].Value);
            Assert.Equal("21", sut[0, 2].Value);
        }

        /// <summary>
        /// Test background color changing.
        /// </summary>
        [Fact]
        public void BackgroundColorChange()
        {
            Spreadsheet sut = new(1, 2);
            sut[0, 0].BackgroundColor = 0xFFFF7F50;
            sut[0, 1].BackgroundColor = 0xFF6495ED;
            Assert.Equal(0xFFFF7F50, sut[0, 0].BackgroundColor);
            Assert.Equal(0xFF6495ED, sut[0, 1].BackgroundColor);
        }

        /// <summary>
        /// Test various inputs into spreadsheet.
        /// </summary>
        /// <param name="expression">The text to be passed into a cell.</param>
        /// <param name="expected">The expected result (value) of the cell.</param>
        [Theory]
        [InlineData("=", "=")]
        public void SpreadsheetCellEvaluateTests(string expression, string expected)
        {
            Spreadsheet sut = new(1, 3);
            sut[0, 1].Text = expression;

            Assert.Equal(expected, sut[0, 1].Value);
        }

        /// <summary>
        /// Test saving xml as a proper format.
        /// </summary>
        [Fact]
        public void SaveSpreadsheetAsXml()
        {
            const string path = "Root.xml";
            Spreadsheet sut = new(2, 3);
            sut[1, 0].Text = "=A1+6";
            sut[1, 0].BackgroundColor = 0xFF8000;
            sut.SaveSpreadsheet(path);
            Assert.Equal(
                "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                "\r\n<Spreadsheet>\r\n" +
                "  <SpreadsheetCell IndexName=\"B1\">\r\n" +
                "    <BackgroundColor>16744448</BackgroundColor>\r\n" +
                "    <Text>=A1+6</Text>\r\n" +
                "  </SpreadsheetCell>\r\n" +
                "</Spreadsheet>",
                File.ReadAllText(path));
        }

        /// <summary>
        /// Test saving xml as a proper format.
        /// </summary>
        [Fact]
        public void SaveSpreadsheetAsXml_GivenDefaultColor_SkipCellSave()
        {
            const string path = "Root.xml";
            Spreadsheet sut = new(2, 3);
            sut[1, 0].BackgroundColor = 0xFFFFFFFF;
            sut.SaveSpreadsheet(path);
            Assert.NotEqual(
                "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                "\r\n<Spreadsheet>\r\n" +
                "  <SpreadsheetCell IndexName=\"B1\">\r\n" +
                "    <BackgroundColor>16744448</BackgroundColor>\r\n" +
                "    <Text>=A1+6</Text>\r\n" +
                "  </SpreadsheetCell>\r\n" +
                "</Spreadsheet>",
                File.ReadAllText(path));
        }

        /// <summary>
        /// Test saving xml as a proper format.
        /// </summary>
        [Fact]
        public void SaveSpreadsheetAsXml_GivenDefaultText_SkipCellSave()
        {
            const string path = "Root.xml";
            Spreadsheet sut = new(2, 3);
            sut[1, 0].Text = string.Empty;
            sut.SaveSpreadsheet(path);
            Assert.NotEqual(
                "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                "\r\n<Spreadsheet>\r\n" +
                "  <SpreadsheetCell IndexName=\"B1\">\r\n" +
                "    <BackgroundColor>16744448</BackgroundColor>\r\n" +
                "    <Text>=A1+6</Text>\r\n" +
                "  </SpreadsheetCell>\r\n" +
                "</Spreadsheet>",
                File.ReadAllText(path));
        }

        /// <summary>
        /// Test saving xml as a proper format.
        /// </summary>
        [Fact]
        public void SaveSpreadsheetAsXml_GivenNoChanges_SkipCellSave()
        {
            const string path = "Root.xml";
            Spreadsheet sut = new(2, 3);
            sut.SaveSpreadsheet(path);
            Assert.NotEqual(
                "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                "\r\n<Spreadsheet>\r\n" +
                "  <SpreadsheetCell IndexName=\"B1\">\r\n" +
                "    <BackgroundColor>16744448</BackgroundColor>\r\n" +
                "    <Text>=A1+6</Text>\r\n" +
                "  </SpreadsheetCell>\r\n" +
                "</Spreadsheet>",
                File.ReadAllText(path));
        }

        /// <summary>
        /// Test saving xml as a proper format.
        /// </summary>
        [Fact]
        public void LoadSpreadsheetFromXml()
        {
            const string path = "Root.xml";
            Spreadsheet sut = new(2, 3);
            sut[1, 0].Text = "=A1+6";
            sut[1, 0].BackgroundColor = 0xFF8000;
            sut.SaveSpreadsheet(path);
            sut = new Spreadsheet(2, 3);
            sut.LoadSpreadsheet(path);
            Assert.Equal("=A1+6", sut[1, 0].Text);
            Assert.Equal(16744448.ToString(), sut[1, 0].BackgroundColor.ToString());
        }

        /// <summary>
        /// Test saving xml as a proper format.
        /// </summary>
        [Fact]
        public void LoadSpreadsheetFromXml_GivenNothing()
        {
            const string path = "Root.xml";
            Spreadsheet sut = new(2, 3);
            sut.SaveSpreadsheet(path);
            sut = new Spreadsheet(2, 3);
            sut.LoadSpreadsheet(path);
            Assert.Equal(string.Empty, sut[1, 0].Text);
        }

        /// <summary>
        /// Test undo a command.
        /// </summary>
        [Fact]
        public void UndoCommand_ToPrevious()
        {
            Spreadsheet sut = new(5, 5);
            sut[0, 0].Text = "=88";
            sut[0, 0].Text = "=12";
            sut.Undo();
            Assert.Equal("=88", sut[0, 0].Text);
            Assert.Equal("88", sut[0, 0].Value);
        }

        /// <summary>
        /// Test undo a command.
        /// </summary>
        [Fact]
        public void UndoCommandToEmpty()
        {
            Spreadsheet sut = new(5, 5);
            sut[0, 0].Text = "=88";
            sut.Undo();
            Assert.Equal(string.Empty, sut[0, 0].Text);
            Assert.Equal(string.Empty, sut[0, 0].Value);
        }

        /// <summary>
        /// Test undo a command when it can't be undone anymore.
        /// </summary>
        [Fact]
        public void UndoCommandNoMoreUndoCommands()
        {
            Spreadsheet sut = new(5, 5);
            sut[0, 0].Text = "=88";
            sut.Undo();
            sut.Undo();
            Assert.Equal(string.Empty, sut[0, 0].Text);
            Assert.Equal(string.Empty, sut[0, 0].Value);
        }

        /// <summary>
        /// Test undo a command when it can't be undone anymore.
        /// </summary>
        [Fact]
        public void SetCellTextToNull()
        {
            Spreadsheet sut = new(5, 5);
            sut[0, 0].Text = null!;
            Assert.Equal(string.Empty, sut[0, 0].Text);
            Assert.Equal(string.Empty, sut[0, 0].Value);
        }

        /// <summary>
        /// Test undo multiple cell color changes at once.
        /// </summary>
        [Fact]
        public void SetMultipleCellBackgroundColors()
        {
            Spreadsheet sut = new(5, 5);
            List<Cell> cells = new List<Cell>()
            {
                sut[0, 0],
                sut[1, 1],
                sut[2, 1],
                sut[2, 3],
                sut[4, 4],
            };
            sut.SetBackgroundColor(cells, 0xFF8000);
            sut.Undo();
            Assert.Equal(0xFFFFFFFF, sut[0, 0].BackgroundColor);
            Assert.Equal(0xFFFFFFFF, sut[1, 1].BackgroundColor);
            Assert.Equal(0xFFFFFFFF, sut[2, 1].BackgroundColor);
            Assert.Equal(0xFFFFFFFF, sut[2, 3].BackgroundColor);
            Assert.Equal(0xFFFFFFFF, sut[4, 4].BackgroundColor);
        }
    }
}

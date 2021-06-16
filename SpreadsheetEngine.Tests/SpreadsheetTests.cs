// <copyright file="SpreadsheetTests.cs" company="Benjamin Michaelis">
// Copyright (c) Benjamin Michaelis. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xunit;

namespace SpreadsheetEngine.Tests
{
    /// <summary>
    /// Test methods in the spreadsheetClass.
    /// </summary>
    public class SpreadsheetTests
    {
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
        /// Test Circular reference, that it displays an error in the cell when a circular reference appears.
        /// </summary>
        [Fact]
        public void CircularReference_GivenCircularReference_SetCellValueToError()
        {
            Spreadsheet sut = new(1, 2);
            sut[0, 0].Text = "=A2";
            sut[0, 1].Text = "=A1";
            Assert.Equal(CircularReferenceException.DefaultMessage, sut[0, 0].Value);
            Assert.Equal(CircularReferenceException.DefaultMessage, sut[0, 1].Value);
        }

        /// <summary>
        /// Test Circular reference, that when you fix the circular reference problem, all the cells get updated.
        /// </summary>
        [Fact]
        public void CircularReference_GivenCircularReferenceError_ResetValues()
        {
            Spreadsheet sut = new(1, 2);
            sut[0, 0].Text = "=A2";
            sut[0, 1].Text = "=A1";

            // Error should be thrown above here as tested elsewhere.
            sut[0, 0].Text = "=10";
            Assert.Equal("10", sut[0, 0].Value);
            Assert.Equal("10", sut[0, 1].Value);
        }

        /// <summary>
        /// Test Circular references when a cell circular reference is multiple "steps" or cells away.
        /// </summary>
        [Fact]
        public void CircularReference_GivenMultipleStepsAway_SetCellValueToError()
        {
            Spreadsheet sut = new(5, 5);
            sut[0, 0].Text = "=B1"; // This = A1
            sut[1, 0].Text = "=B2"; // This = B1
            sut[1, 1].Text = "=A2"; // This = B2
            sut[0, 1].Text = "=A1"; // This = A2
            Assert.Equal(CircularReferenceException.DefaultMessage, sut[0, 0].Value); // This = A1
            Assert.Equal(CircularReferenceException.DefaultMessage, sut[1, 0].Value); // This = B1
            Assert.Equal(CircularReferenceException.DefaultMessage, sut[1, 1].Value); // This = B2
            Assert.Equal(CircularReferenceException.DefaultMessage, sut[0, 1].Value); // This = A2
        }

        /// <summary>
        /// Test Circular references when a cell circular reference is multiple "steps" or cells away, that when you fix the circular problem, all the cells get updated.
        /// </summary>
        [Fact]
        public void CircularReference_GivenCircularReferenceMultipleStepsAway_ResetValues()
        {
            Spreadsheet sut = new(5, 5);
            sut[0, 0].Text = "=B1"; // This = A1
            sut[1, 0].Text = "=B2"; // This = B1
            sut[1, 1].Text = "=A2"; // This = B2
            sut[0, 1].Text = "=A1"; // This = A2

            // Error should be thrown above here as tested elsewhere.
            sut[0, 0].Text = "=10";
            Assert.Equal("10", sut[0, 0].Value); // This = A1
            Assert.Equal("10", sut[1, 0].Value); // This = B1
            Assert.Equal("10", sut[1, 1].Value); // This = B2
            Assert.Equal("10", sut[0, 1].Value); // This = A2
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
        /// Test Referencing a cell that doesn't exist.
        /// </summary>
        [Fact]
        public void Indexer_GivenNonexistentIndex_ThrowIndexOutOfRangeException()
        {
            Spreadsheet sut = new(1, 2);
            Assert.Throws<IndexOutOfRangeException>(() => _ = sut[0, 42]);
        }

        /// <summary>
        /// Test Referencing a cell that doesn't exist.
        /// </summary>
        [Fact]
        public void ReferenceNonExistingCell()
        {
            Spreadsheet sut = new(1, 2);
            sut[0, 0].Text = "=ZZ1";
            Assert.Equal(Cell.CellErrorMessage, sut[0, 0].Value);
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
        /// Check that the valid cell name regex works.
        /// </summary>
        [Fact]
        public void ValidCellNameRegexWorks()
        {
            // https://regexr.com/5r9fa
            Regex lettersThenNumbersRegex = new(@"[A-Za-z]+\d+$");
            Assert.Matches(lettersThenNumbersRegex, "A1");
    }

        /// <summary>
        /// If cell name is valid, return true from method.
        /// </summary>
        [Fact]
        public void IsValidCellName_GivenValidCellName_ReturnTrue()
        {
            Spreadsheet sut = new(1, 1);
            Assert.True(sut.IsValidCellName("A1"));
        }

        /// <summary>
        /// If cell name is valid, return true from method.
        /// </summary>
        [Fact]
        public void IsValidCellName_GivenValidLowercaseCellName_ReturnTrue()
        {
            Spreadsheet sut = new(1, 1);
            Assert.True(sut.IsValidCellName("a1"));
        }

        /// <summary>
        /// Test that when given the letter AH for a column number, it false.
        /// </summary>
        [Fact]
        public void IsValidCellName_AH_returnsFalse()
        {
            Spreadsheet sut = new(1, 3);
            Assert.False(sut.IsValidCellName("AH"));
        }

        /// <summary>
        /// Test that when given the letters lowercase ah for a column number, it returns true.
        /// </summary>
        [Fact]
        public void IsValidCellName_ah_returnFalse()
        {
            Spreadsheet sut = new(1, 3);
            Assert.False(sut.IsValidCellName("ah"));
        }

        /// <summary>
        /// Test that when given the letter 1AH for a column number, it returns false (invalid string).
        /// </summary>
        [Fact]
        public void IsValidCellName_1AH_returnFalse()
        {
            Spreadsheet sut = new(1, 3);
            Assert.False(sut.IsValidCellName("1AH"));
        }

        /// <summary>
        /// Test that when given the letter AH11A for a column number, it returns false (invalid string).
        /// </summary>
        [Fact]
        public void IsValidCellName_AH11A_returnFalse()
        {
            Spreadsheet sut = new(1, 3);
            Assert.False(sut.IsValidCellName("AH11A"));
        }

        /// <summary>
        /// Test that when given the letter * for a column number, it returns false (invalid string).
        /// </summary>
        [Fact]
        public void IsValidCellName_Star_returnFalse()
        {
            Spreadsheet sut = new(1, 3);
            Assert.False(sut.IsValidCellName("*"));
        }

        /// <summary>
        /// Test that when given the letter * for a column number, it returns false (invalid string).
        /// </summary>
        [Fact]
        public void IsValidCellName_SZ1InRange_returnTrue()
        {
            // 'SZ' is column number 520
            Spreadsheet sut = new(520, 1);
            Assert.True(sut.IsValidCellName("SZ1"));
        }

        /// <summary>
        /// If cell name is out of range, return false from method.
        /// </summary>
        [Fact]
        public void IsValidCellName_GivenOutOfRangeCellName_ReturnFalse()
        {
            Spreadsheet sut = new(1, 1);
            Assert.False(sut.IsValidCellName("SZ1"));
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
        /// Test undo multiple commands.
        /// </summary>
        [Fact]
        public void UndoMultipleCommandsToPreviousState()
        {
            Spreadsheet sut = new(5, 5);
            sut[0, 0].Text = "=88";
            sut[1, 1].Text = "=99";
            sut.Undo();
            sut.Undo();
            Assert.Equal(string.Empty, sut[0, 0].Text);
            Assert.Equal(string.Empty, sut[0, 0].Value);
            Assert.Equal(string.Empty, sut[1, 1].Text);
            Assert.Equal(string.Empty, sut[1, 1].Value);
        }

        /// <summary>
        /// Test undo multiple commands.
        /// </summary>
        [Fact]
        public void UndoMultipleTypesOfCommandsToPreviousState()
        {
            Spreadsheet sut = new(5, 5);
            sut[0, 0].Text = "=88";
            sut[1, 1].Text = "=99";
            List<Cell> cells = new()
            {
                sut[0, 0],
                sut[1, 1],
                sut[2, 1],
                sut[2, 3],
                sut[4, 4],
            };
            sut.SetBackgroundColor(cells, 0xFF8000);
            sut.Undo();
            sut.Undo();
            sut.Undo();
            Assert.Equal(string.Empty, sut[0, 0].Text);
            Assert.Equal(string.Empty, sut[0, 0].Value);
            Assert.Equal(string.Empty, sut[1, 1].Text);
            Assert.Equal(string.Empty, sut[1, 1].Value);
            Assert.Equal(0xFFFFFFFF, sut[0, 0].BackgroundColor);
            Assert.Equal(0xFFFFFFFF, sut[1, 1].BackgroundColor);
            Assert.Equal(0xFFFFFFFF, sut[2, 1].BackgroundColor);
            Assert.Equal(0xFFFFFFFF, sut[2, 3].BackgroundColor);
            Assert.Equal(0xFFFFFFFF, sut[4, 4].BackgroundColor);
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
            List<Cell> cells = new()
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

        /// <summary>
        /// Test redo a command.
        /// </summary>
        [Fact]
        public void RedoCommandToPreviousState()
        {
            Spreadsheet sut = new(5, 5);
            sut[0, 0].Text = "=88";
            sut.Undo();
            sut.Redo();
            Assert.Equal("=88", sut[0, 0].Text);
            Assert.Equal("88", sut[0, 0].Value);
        }

        /// <summary>
        /// Test that the redo stack clears after performing another operation.
        /// </summary>
        [Fact]
        public void RedoCommandStackClear()
        {
            Spreadsheet sut = new(5, 5);
            sut[0, 0].Text = "=88";
            sut.Undo();
            sut[1, 1].Text = "=99";
            sut.Redo();
            Assert.Equal(string.Empty, sut[0, 0].Text);
            Assert.Equal(string.Empty, sut[0, 0].Value);
            Assert.Equal("=99", sut[1, 1].Text);
            Assert.Equal("99", sut[1, 1].Value);
        }

        /// <summary>
        /// Test redo a command.
        /// </summary>
        [Fact]
        public void RedoMultipleCommandsToPreviousState()
        {
            Spreadsheet sut = new(5, 5);
            sut[0, 0].Text = "=88";
            sut[1, 1].Text = "=99";
            sut.Undo();
            sut.Undo();
            sut.Redo();
            sut.Redo();
            Assert.Equal("=88", sut[0, 0].Text);
            Assert.Equal("88", sut[0, 0].Value);
            Assert.Equal("=99", sut[1, 1].Text);
            Assert.Equal("99", sut[1, 1].Value);
        }
    }
}

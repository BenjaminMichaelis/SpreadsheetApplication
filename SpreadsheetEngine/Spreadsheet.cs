// <copyright file="Spreadsheet.cs" company="Benjamin Michaelis">
// Copyright (c) Benjamin Michaelis. ID: 11620581. All rights reserved.
// </copyright>

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadsheetEngine
{
    /// <summary>
    /// Is a container of a 2D array of cells.
    /// </summary>
    public class Spreadsheet
    {
        private class SpreadsheetCell : Cell
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="SpreadsheetCell"/> class.
            /// </summary>
            /// <param name="rowIndex">Row index of cell.</param>
            /// <param name="columnIndex">Column index of cell.</param>
            public SpreadsheetCell(int rowIndex, int columnIndex, Spreadsheet spreadsheet)
                : base(rowIndex, columnIndex)
            {
                this.PropertyChanged += this.CellPropertyChanged;
                this.SpreadsheetReference = spreadsheet;
            }

            private Spreadsheet SpreadsheetReference { get; set; }

            private Dictionary<Cell, PropertyChangedEventHandler> ReferencedCells { get; } = new();

            public void SetCellValue(string value)
            {
                // this. or base.
                this.InternalValue = value;
            }

            /// <summary>
            /// Notifies for  when any property for any cell in the worksheet has changed.
            /// </summary>
            /// <param name="sender">Object that called object.</param>
            /// <param name="e">The property changed arg.</param>
            internal void CellPropertyChanged(object? sender, PropertyChangedEventArgs e)
            {
                if (sender is Cell evaluatingCell)
                {
                    if (e.PropertyName == nameof(Cell.Text))
                    {
                        foreach (KeyValuePair<Cell, PropertyChangedEventHandler> item in this.ReferencedCells)
                        {
                            item.Key.PropertyChanged -= item.Value;
                        }

                        this.ReferencedCells.Clear();
                        if (!string.IsNullOrEmpty(this.Text))
                        {
                            // If evaluating cell text starts with = then we will have to evaluate all the text to set the value appropriately.
                            if (this.Text.StartsWith("="))
                            {
                                string evaluatedString = this.Text.Substring(1);
                                ExpressionTree newEvaluationTree = new(evaluatedString);
                                foreach (KeyValuePair<string, double> keyValuePair in newEvaluationTree.Values)
                                {
                                    try
                                    {
                                        SpreadsheetCell variableCell = this.SpreadsheetReference.GetCell(keyValuePair.Key);
                                        if(this.ReferencedCells.Keys.Contains(variableCell))
                                        {
                                            throw new CircularReferenceException();
                                        }

                                        PropertyChangedEventHandler eventHandler = new(this.CellPropertyChanged);
                                        this.ReferencedCells.Add(variableCell, eventHandler);
                                        variableCell.PropertyChanged += eventHandler;
                                    }
                                    catch (CircularReferenceException exception)
                                    {
                                        this.SetCellValue(exception.Message);
                                        return;
                                    }
                                    catch (ArgumentNullException)
                                    {
                                        this.SetCellValue(CircularReference);
                                        return;
                                    }
                                    catch (Exception)
                                    {
                                        this.SetCellValue(CircularReference);
                                        return;
                                    }
                                }

                                foreach (KeyValuePair<string, double> keyValuePair in newEvaluationTree.Values)
                                {
                                    try
                                    {
                                        newEvaluationTree.SetVariable(
                                            keyValuePair.Key,
                                            double.Parse(this.SpreadsheetReference.GetCell(keyValuePair.Key).Value));
                                    }
                                    catch (NullReferenceException)
                                    {
                                        this.SetCellValue(CircularReference);
                                        return;
                                    }
                                    catch (ArgumentNullException)
                                    {
                                        this.SetCellValue(CircularReference);
                                        return;
                                    }
                                    catch (Exception)
                                    {
                                        this.SetCellValue(CircularReference);
                                        return;
                                    }
                                }

                                evaluatedString = newEvaluationTree.Evaluate().ToString();
                                this.SetCellValue(evaluatedString);
                            }
                            else
                            {
                                this.SetCellValue(this.Text);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Event handler to connect to UI level.
        /// </summary>
        public event PropertyChangedEventHandler? OnCellPropertyChanged;

        private SpreadsheetCell[,] _cellsOfSpreadsheet;

        /// <summary>
        /// Initializes a new instance of the <see cref="Spreadsheet"/> class.
        /// </summary>
        /// <param name="rows">Number of rows.</param>
        /// <param name="columns">Number of columns.</param>
        public Spreadsheet(int rows, int columns)
        {
            this.ColumnCount = columns;
            this.RowCount = rows;
            this._cellsOfSpreadsheet = new SpreadsheetCell[rows, columns];
            for (int rowNum = 0; rowNum < rows; rowNum++)
            {
                for (int colNum = 0; colNum < columns; colNum++)
                {
                    this._cellsOfSpreadsheet[rowNum, colNum] = new SpreadsheetCell(rowNum, colNum, this);
#pragma warning disable CS8622 // Nullability of reference types in type of parameter doesn't match the target delegate (possibly because of nullability attributes).
                    this._cellsOfSpreadsheet[rowNum, colNum].PropertyChanged += this.CellPropertyChanged;
#pragma warning disable CS8622 // Nullability of reference types in type of parameter doesn't match the target delegate (possibly because of nullability attributes).

                }
            }

            // this._cellsOfSpreadsheet[rownum, colnum].PropertyChanged += this.CellPropertyChanged;
        }

        /// <summary>
        /// Converts the first letters in a string to numbers.
        /// </summary>
        /// <param name="cellName">The name of the cell (ex: AA33).</param>
        /// <returns>Int of SpreadsheetCell.</returns>
        public int ColumnLetterToInt(string cellName)
        {
            string columnLetters = string.Concat(cellName.TakeWhile(char.IsLetter));
            int columnLocation = columnLetters.ToCharArray().Select(c => c - 'A' + 1).Reverse().Select((v, i) => v * (int)Math.Pow(26, i)).Sum();
            return columnLocation;
        }

        /// <summary>
        /// Runs a demo of the code with 50 cells displaying "Hello World".
        /// </summary>
        public void Demo()
        {
            int randomRow = 0, randomCol = 0;
            Random random = new Random();

            for (int i = 0; i < 50; i++)
            {
                randomCol = random.Next(0, 25);
                randomRow = random.Next(0, 49);

                SpreadsheetCell temp = this.GetCell(randomRow!, randomCol!);
                temp!.Text = "Hello World";
                this._cellsOfSpreadsheet[randomRow, randomCol] = temp;
            }

            for (int i = 0; i < 50; i++)
            {
                this._cellsOfSpreadsheet[i, 1].Text = $"This is SpreadsheetCell B{(i + 1).ToString()}";
            }
        }

        /// <summary>
        /// Notifies for  when any property for any cell in the worksheet has changed.
        /// </summary>
        /// <param name="sender">Object that called object.</param>
        /// <param name="e">The property changed arg.</param>
        public void CellPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (sender is SpreadsheetCell evaluatingCell)
            {
                if (e.PropertyName == nameof(Cell.Text))
                {
                    if (!string.IsNullOrEmpty(evaluatingCell.Text))
                    {
                        this.OnCellPropertyChanged?.Invoke(sender, e);
                    }
                }
                else if (e.PropertyName == nameof(Cell.Value))
                {
                    if (!string.IsNullOrEmpty(evaluatingCell.Value))
                    {
                        this.OnCellPropertyChanged?.Invoke(sender, e);
                    }
                }
                else
                {
                    throw new NotImplementedException("Spreadsheet cell property changed not implemented else statement");
                }
            }
        }

        /// <summary>
        /// GetCell function that takes a row and column index and returns the cell at that location or null if there is no such cell.
        /// </summary>
        /// <param name="rowIndex">Pass in the row of the cell you want to access.</param>
        /// <param name="colIndex">Pass in the column of the cell you want to access.</param>
        /// <returns>Returns a SpreadsheetCell of SpreadsheetCell.</returns>
        public string? GetCellText(int rowIndex, int colIndex)
        {
            if ((rowIndex <= this.RowCount && rowIndex >= 0) && colIndex <= this.ColumnCount && colIndex >= 0)
            {
                return this._cellsOfSpreadsheet[rowIndex, colIndex].Text;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// GetCell function that takes a row and column index and returns the cell at that location or null if there is no such cell.
        /// </summary>
        /// <param name="rowIndex">Pass in the row of the cell you want to access.</param>
        /// <param name="colIndex">Pass in the column of the cell you want to access.</param>
        /// <returns>Returns a SpreadsheetCell of SpreadsheetCell.</returns>
        public string? GetCellValue(int rowIndex, int colIndex)
        {
            if ((rowIndex <= this.RowCount && rowIndex >= 0) && colIndex <= this.ColumnCount && colIndex >= 0)
            {
                return this._cellsOfSpreadsheet[rowIndex, colIndex].Value;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// GetCell function that takes a row and column index and returns the text at that location or null if there is no such cell.
        /// </summary>
        /// <param name="rowIndex">Pass in the row of the cell you want to access.</param>
        /// <param name="colIndex">Pass in the column of the cell you want to access.</param>
        /// <returns>Returns a string of the cell.</returns>
        private SpreadsheetCell? GetCell(int rowIndex, int colIndex)
        {
            if ((rowIndex <= this.RowCount && rowIndex >= 0) && colIndex <= this.ColumnCount && colIndex >= 0)
            {
                return this._cellsOfSpreadsheet[rowIndex, colIndex];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Get the cell from the cell name.
        /// </summary>
        /// <param name="cellName">Letter/number combo of cell.</param>
        /// <returns>Returns cell in spreadsheet that matches the index of the cell.</returns>
        private SpreadsheetCell? GetCell(string cellName)
        {
            int columnLocation = this.ColumnLetterToInt(cellName);
            string rowLocationString = string.Join(null, System.Text.RegularExpressions.Regex.Split(cellName, "[^\\d]"));
            int rowLocation = int.Parse(rowLocationString);
            return this.GetCell(rowLocation - 1, columnLocation - 1);
        }

        /// <summary>
        /// Gets or sets returns number of columns in spreadsheet.
        /// </summary>
        public int ColumnCount { get; set; }

        /// <summary>
        /// Gets or sets returns number of rows in spreadsheet.
        /// </summary>
        public int RowCount { get; set; }

        /// <summary>
        /// Sets the specified cells text.
        /// </summary>
        /// <param name="rowIndex">rowIndex.</param>
        /// <param name="columnIndex">columnIndex.</param>
        /// <param name="newCellText">the new text to set the cell to.</param>
        public void SetCellText(int rowIndex, int columnIndex, string newCellText)
        {
            this._cellsOfSpreadsheet[rowIndex, columnIndex].Text = newCellText;
        }
    }
}

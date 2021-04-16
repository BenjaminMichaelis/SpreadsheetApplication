// <copyright file="SpreadsheetCell.cs" company="Benjamin Michaelis">
// Copyright (c) Benjamin Michaelis. ID: 11620581. All rights reserved.
// </copyright>

using System;
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
    public partial class Spreadsheet
    {
        private class SpreadsheetCell : Cell
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="SpreadsheetCell"/> class.
            /// </summary>
            /// <param name="columnIndex">Column index of cell.</param>
            /// <param name="rowIndex">Row index of cell.</param>
            /// <param name="spreadsheet">Spreadsheet reference that the cell is a part of.</param>
            public SpreadsheetCell(int columnIndex, int rowIndex, Spreadsheet spreadsheet)
                : base(columnIndex, rowIndex)
            {
                this.PropertyChanged += this.CellPropertyChanged;
                this.SpreadsheetReference = spreadsheet;
            }

            public Dictionary<SpreadsheetCell, PropertyChangedEventHandler> ReferencedCells { get; } = new();

            private Spreadsheet SpreadsheetReference { get; set; }

            private void SetCellValue(string value)
            {
                // this. or base.
                this.InternalValue = value;
            }

            /// <summary>
            /// Notifies for  when any property for any cell in the worksheet has changed.
            /// </summary>
            /// <param name="sender">Object that called object.</param>
            /// <param name="e">The property changed arg.</param>
            private void CellPropertyChanged(object? sender, PropertyChangedEventArgs e)
            {
                switch (sender)
                {
                    case SpreadsheetCell evaluatingCell:
                    {
                        switch (e.PropertyName)
                        {
                            case nameof(Cell.Text):
                            case nameof(Cell.Value) when this != evaluatingCell:
                            {
                                if (!string.IsNullOrEmpty(this.Text))
                                {
                                    // If evaluating cell text starts with = then we will have to evaluate all the text to set the value appropriately.
                                    if (this.Text.StartsWith("=") && this.Text.Length > 1)
                                    {
                                        try
                                        {
                                            string evaluatedString = this.Text[1..];
                                            ExpressionTree newEvaluationTree = new(evaluatedString);
                                            IEnumerable<SpreadsheetCell?> referencedCells = newEvaluationTree.Values.Select(
                                                item => this.SpreadsheetReference[item.Key]
                                            );
                                            referencedCells = referencedCells.Where(
                                                item => item is { }
                                            );
                                            IEnumerable<SpreadsheetCell?> intersectingCells = referencedCells.Intersect(evaluatingCell.ReferencedCells.Keys);
                                            foreach (SpreadsheetCell? item in intersectingCells)
                                            {
                                                if (item != null)
                                                {
                                                    item.ErrorMessage = CircularReferenceException.DefaultMessage;
                                                }
                                            }

                                            if (intersectingCells.Any())
                                            {
                                                throw new CircularReferenceException();
                                            }

                                            foreach (SpreadsheetCell item in referencedCells)
                                            {
                                                if (item != null)
                                                {
                                                    item.ErrorMessage = null;
                                                }
                                            }

                                            foreach (SpreadsheetCell item in this.ReferencedCells.Keys)
                                            {
                                                item.ErrorMessage = null;
                                            }

                                            foreach (KeyValuePair<SpreadsheetCell, PropertyChangedEventHandler> item in this.ReferencedCells)
                                            {
                                                item.Key.PropertyChanged -= item.Value;
                                            }

                                            this.ReferencedCells.Clear();

                                            foreach (SpreadsheetCell? item in referencedCells)
                                            {
                                                try
                                                {
                                                    SpreadsheetCell variableCell = item ?? throw new InvalidOperationException();

                                                    PropertyChangedEventHandler eventHandler = new(this.CellPropertyChanged);
                                                    this.ReferencedCells.Add(variableCell, eventHandler);
                                                    variableCell.PropertyChanged += eventHandler;
                                                }
                                                catch (InvalidOperationException)
                                                {
                                                    this.SetCellValue("#error: reference cell is null");
                                                    return;
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
                                                        double.Parse(this.SpreadsheetReference[keyValuePair.Key].Value));
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
                                        catch (CircularReferenceException exception)
                                        {
                                            this.ErrorMessage = exception.Message;
                                        }
                                        catch (Exception exception)
                                        {
                                            Console.WriteLine(exception);
                                            throw;
                                        }
                                    }
                                    else
                                    {
                                        this.SetCellValue(this.Text);
                                    }
                                }

                                break;
                            }

                            case nameof(Cell.BackgroundColor):
                                break;
                        }

                        break;
                    }
                }
            }
        }
    }
}

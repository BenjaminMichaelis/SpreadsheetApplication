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

            public override Cell Clone()
            {
                SpreadsheetCell cellCopy = new(this.ColumnIndex, this.RowIndex, this.SpreadsheetReference)
                {
                    BackgroundColor = this.BackgroundColor,
                    Text = this.Text,
                    ErrorMessage = this.ErrorMessage,
                };
                return cellCopy;
            }

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
                                this.UpdateCellValue();

                                break;
                            }

                            case nameof(Cell.BackgroundColor):
                                break;
                        }

                        break;
                    }
                }
            }

            public void UpdateCellValue()
            {
                bool wasErrored = this.IsErrored;
                // If evaluating cell text starts with = then we will have to evaluate all the text to set the value appropriately.
                if (this.Text.StartsWith("=") && this.Text.Length > 1)
                {
                    try
                    {
                        IEnumerable<SpreadsheetCell?> referencedCells;
                        string evaluatedString = this.Text[1..];
                        ExpressionTree newEvaluationTree = new(evaluatedString);
                        if (newEvaluationTree.Values.Any(item =>
                            !this.SpreadsheetReference.IsValidCellName(item.Key)
                        ))
                        {
                            this.ErrorMessage = Cell.CellErrorMessage;
                            return;
                        }

                        referencedCells = newEvaluationTree.Values.Select(
                            item => this.SpreadsheetReference[item.Key]
                        );
                        referencedCells = referencedCells.Where(
                            item => item is { }
                        );

                        this.ErrorMessage = null;

                        bool circularReference = this.SpreadsheetReference.IsCalculating.Contains(this);
                        if (circularReference)
                        {
                            foreach (SpreadsheetCell cell in this.SpreadsheetReference.IsCalculating)
                            {
                                cell.ErrorMessage = CircularReferenceException.DefaultMessage;
                            }

                            return;
                        }

                        this.SpreadsheetReference.IsCalculating.Add(this);

                        foreach (SpreadsheetCell cell in referencedCells)
                        {
                            cell?.UpdateCellValue();
                        }

                        foreach (KeyValuePair<SpreadsheetCell, PropertyChangedEventHandler> item in this
                            .ReferencedCells)
                        {
                            item.Key.PropertyChanged -= item.Value;
                        }

                        this.ReferencedCells.Clear();

                        foreach (SpreadsheetCell? item in referencedCells)
                        {
                            try
                            {
                                SpreadsheetCell variableCell =
                                    item ?? throw new InvalidOperationException();

                                PropertyChangedEventHandler
                                    eventHandler = new(this.CellPropertyChanged);
                                this.ReferencedCells.Add(variableCell, eventHandler);
                                variableCell.PropertyChanged += eventHandler;
                            }
                            catch (InvalidOperationException)
                            {
                                this.SetCellValue("#error: reference cell is null");
                                return;
                            }

                            foreach (KeyValuePair<string, double> keyValuePair in newEvaluationTree.Values)
                            {
                                try
                                {
                                    bool valueTryParse = double.TryParse(
                                        this.SpreadsheetReference[keyValuePair.Key].Value,
                                        out double cellValue);
                                    switch (valueTryParse)
                                    {
                                        case true:
                                            newEvaluationTree.SetVariable(
                                                keyValuePair.Key,
                                                cellValue);
                                            break;
                                        case false:
                                            newEvaluationTree.SetVariable(
                                                keyValuePair.Key,
                                                0);
                                            break;
                                    }
                                }
                                catch (InvalidOperationException)
                                {
                                    this.SetCellValue("#error: reference cell is null");
                                    return;
                                }
                            }
                        }

                        evaluatedString = newEvaluationTree.Evaluate().ToString();
                        this.SetCellValue(evaluatedString);
                    }
                    catch (CircularReferenceException)
                    {
                        this.ErrorMessage = CircularReferenceException.DefaultMessage;
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
        }
    }
}

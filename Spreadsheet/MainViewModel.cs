// <copyright file="MainViewModel.cs" company="Benjamin Michaelis">
// Copyright (c) Benjamin Michaelis. ID: 11620581. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SpreadsheetEngine;

namespace SpreadsheetApp
{
    /// <summary>
    /// ViewModel.
    /// </summary>
    public class MainViewModel
    {
        private SpreadsheetEngine.Spreadsheet Sheet { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MainViewModel"/> class.
        /// ViewModel - connects Model and View. This layer is an abstraction of the View that exposes public properties and commands used to bind your data to GUI elements and manage this data.
        /// </summary>
        /// <param name="mainForm">pass in the Form to bind.</param>
        public MainViewModel(MainForm mainForm)
        {
            char[] alphabet = Enumerable.Range('A', 'Z' - 'A' + 1).Select(i => (char)i).ToArray();
            this.MainForm = mainForm;

#pragma warning disable CS8622 // Nullability of reference types in type of parameter doesn't match the target delegate (possibly because of nullability attributes).
            this.MainForm.DemoButton.Click += new System.EventHandler(this.DemoButton_Click);
            this.MainForm.changeBackgroundColorButton.Click += new System.EventHandler(this.BackgroundColorButton_Click);
#pragma warning restore CS8622 // Nullability of reference types in type of parameter doesn't match the target delegate (possibly because of nullability attributes).

            this.Sheet = new(50, 26);
#pragma warning disable CS8622 // Nullability of reference types in type of parameter doesn't match the target delegate (possibly because of nullability attributes).
            this.Sheet.OnCellPropertyChanged += this.UpdateCell;
#pragma warning disable CS8622 // Nullability of reference types in type of parameter doesn't match the target delegate (possibly because of nullability attributes).
            this.MainForm.spreadsheetViewUI.Columns.Clear();
            foreach (char c in alphabet)
            {
                this.MainForm.spreadsheetViewUI.Columns.Add(c.ToString(), c.ToString());
            }

            this.MainForm.spreadsheetViewUI.Rows.Clear();
            this.MainForm.spreadsheetViewUI.Rows.Add(50);
            for (int rowNumber = 0; rowNumber < 50; rowNumber++)
            {
                this.MainForm.spreadsheetViewUI.Rows[rowNumber].HeaderCell.Value = string.Format($"{this.MainForm.spreadsheetViewUI.Rows[rowNumber].Index + 1}");
            }

            this.MainForm.spreadsheetViewUI.CellBeginEdit += this.DataGridView1_CellBeginEdit;
            this.MainForm.spreadsheetViewUI.CellEndEdit += this.DataGridView1_CellEndEdit;
        }

        /// <summary>
        /// Runs DemoButton.
        /// </summary>
        /// <param name="sender">sender object.</param>
        /// <param name="e">Property changed event.</param>
        public void DemoButton_Click(object sender, System.EventArgs e)
        {
            this.Sheet.Demo();
        }

        /// <summary>
        /// Runs when the change background color button is clicked.
        /// </summary>
        /// <param name="sender">The sending object.</param>
        /// <param name="e">The event sending argument.</param>
        public void BackgroundColorButton_Click(object sender, System.EventArgs e)
        {
            ColorDialog colorDialog = new ColorDialog();
            colorDialog.AllowFullOpen = true;
            colorDialog.ShowHelp = true;
            colorDialog.AnyColor = true;
            colorDialog.HelpRequest += new System.EventHandler(this.ColorDialog_HelpRequest);

            // if user selects OK in the color dialog, otherwise do nothing.
            if (colorDialog.ShowDialog() != DialogResult.OK) return;
            uint chosenColor = (uint)colorDialog.Color.ToArgb();

            foreach (DataGridViewCell cell in this.MainForm.spreadsheetViewUI.SelectedCells)
            {
                this.Sheet[cell.RowIndex, cell.ColumnIndex].BackgroundColor = (uint)chosenColor;
            }
        }

        private void ColorDialog_HelpRequest(object sender, System.EventArgs e) => MessageBox.Show("Please select a color by clicking it. This will change the background color of a cell box.");

        private void UpdateCell(object sender, PropertyChangedEventArgs e)
        {
            switch (sender)
            {
                case Cell senderCell:
                {
                    switch (e.PropertyName)
                    {
                        case nameof(Cell.Value):
                        case nameof(Cell.Text):
                            this.MainForm.spreadsheetViewUI.Rows[senderCell.RowIndex].Cells[senderCell.ColumnIndex].Value = senderCell.Value;
                            break;
                        case nameof(Cell.BackgroundColor):
                            this.MainForm.spreadsheetViewUI.Rows[senderCell.RowIndex].Cells[senderCell.ColumnIndex].Style.BackColor = System.Drawing.Color.FromArgb((int)senderCell.BackgroundColor);
                            break;
                    }

                    break;
                }
            }
        }

        /// <summary>
        /// While the cell is being edited, gets set to text value.
        /// </summary>
        /// <param name="sender">sender object.</param>
        /// <param name="e">Property changed event.</param>
        private void DataGridView1_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            string? cellText = this.Sheet[e.RowIndex, e.ColumnIndex].Text;

            if (cellText != null)
            {
                this.MainForm.spreadsheetViewUI.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = cellText;
            }
        }

        /// <summary>
        /// After cell is edited, returns to showing the value again.
        /// </summary>
        /// <param name="sender">sender object.</param>
        /// <param name="e">Property changed event.</param>
        private void DataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            string? newCellText = this.MainForm.spreadsheetViewUI.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();

            try
            {
                if (newCellText == null)
                {
                    return;
                }

                switch (newCellText.Length)
                {
                    case > 0:
                        {
                            if (newCellText.StartsWith("="))
                            {
                                this.Sheet[e.RowIndex, e.ColumnIndex].Text = newCellText;
                                this.MainForm.spreadsheetViewUI.Rows[e.RowIndex].Cells[e.ColumnIndex].Value =
                                    this.Sheet[e.RowIndex, e.ColumnIndex].Value;
                            }

                            break;
                        }
                }
            }
            catch (NullReferenceException)
            {
                this.MainForm.spreadsheetViewUI.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = string.Empty;
            }
            catch (Exception)
            {
                throw new Exception("cellValue has been set to null or empty unexpectedly during cell editing.");
            }
        }

        private MainForm MainForm { get; }
    }
}

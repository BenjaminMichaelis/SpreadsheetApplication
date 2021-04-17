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
        private Spreadsheet Sheet { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MainViewModel"/> class.
        /// ViewModel - connects Model and View. This layer is an abstraction of the View that exposes public properties and commands used to bind your data to GUI elements and manage this data.
        /// </summary>
        /// <param name="mainForm">pass in the Form to bind.</param>
        public MainViewModel(MainForm mainForm)
        {
            this.MainForm = mainForm;

#pragma warning disable CS8622 // Nullability of reference types in type of parameter doesn't match the target delegate (possibly because of nullability attributes).
            this.MainForm.DemoButton.Click += new System.EventHandler(this.DemoButton_Click);
            this.MainForm.changeBackgroundColorButton.Click += new System.EventHandler(this.BackgroundColorButton_Click);
            this.MainForm.UndoButton.Click += new System.EventHandler(this.UndoButton_Click);
            this.MainForm.RedoButton.Click += new System.EventHandler(this.RedoButton_Click);
            this.MainForm.LoadButton.Click += new System.EventHandler(this.LoadButton_Click);
            this.MainForm.SaveButton.Click += new System.EventHandler(this.SaveButton_Click);
#pragma warning restore CS8622 // Nullability of reference types in type of parameter doesn't match the target delegate (possibly because of nullability attributes).

            this.Sheet = new SpreadsheetEngine.Spreadsheet(26, 50);
#pragma warning disable CS8622 // Nullability of reference types in type of parameter doesn't match the target delegate (possibly because of nullability attributes).
            this.Sheet.OnCellPropertyChanged += this.UpdateCell;
#pragma warning disable CS8622 // Nullability of reference types in type of parameter doesn't match the target delegate (possibly because of nullability attributes).
            this.InitializeUiGrid();

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
        /// Runs when the Load button is clicked.
        /// </summary>
        /// <param name="sender">The sending object.</param>
        /// <param name="e">The event sending argument.</param>
        public void LoadButton_Click(object sender, System.EventArgs e)
        {
            OpenFileDialog fileDialog = new();

            fileDialog.Filter = "xml files (*.xml)|*.xml";
            fileDialog.RestoreDirectory = true;
            fileDialog.CheckFileExists = true;
            fileDialog.Multiselect = false;

            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                this.InitializeUiGrid();
                this.Sheet.LoadSpreadsheet(fileDialog.OpenFile());
            }
        }

        /// <summary>
        /// Runs when the Save button is clicked.
        /// </summary>
        /// <param name="sender">The sending object.</param>
        /// <param name="e">The event sending argument.</param>
        public void SaveButton_Click(object sender, System.EventArgs e)
        {
            SaveFileDialog fileDialog = new();

            fileDialog.Filter = "xml files (*.xml)|*.xml";
            fileDialog.RestoreDirectory = true;
            fileDialog.ValidateNames = true;

            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                this.Sheet.SaveSpreadsheet(fileDialog.OpenFile());
            }
        }

        /// <summary>
        /// Runs when the change background color button is clicked.
        /// </summary>
        /// <param name="sender">The sending object.</param>
        /// <param name="e">The event sending argument.</param>
        public void BackgroundColorButton_Click(object sender, System.EventArgs e)
        {
            using ColorDialog colorDialog = new ColorDialog
            {
                AllowFullOpen = true,
                ShowHelp = true,
                AnyColor = true,
            };
            colorDialog.HelpRequest += new System.EventHandler(this.ColorDialog_HelpRequest);

            // if user selects OK in the color dialog, otherwise do nothing.
            if (colorDialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            uint chosenColor = (uint)colorDialog.Color.ToArgb();

            IEnumerable<Cell> cells = this.MainForm.spreadsheetViewUI.SelectedCells.Cast<DataGridViewCell>().Select(item =>
                this.Sheet[item.ColumnIndex, item.RowIndex]);
            this.Sheet.SetBackgroundColor(cells, chosenColor);
        }

        /// <summary>
        /// Runs when the undo button is clicked.
        /// </summary>
        /// <param name="sender">The object that is triggering this.</param>
        /// <param name="e">The event handler.</param>
        public void UndoButton_Click(object sender, System.EventArgs e)
        {
            this.Sheet.Undo();
        }

        /// <summary>
        /// Runs when the redo button is clicked.
        /// </summary>
        /// <param name="sender">The sending object.</param>
        /// <param name="e">The event sending argument.</param>
        public void RedoButton_Click(object sender, System.EventArgs e)
        {
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
            string? cellText = this.Sheet[e.ColumnIndex, e.RowIndex].Text;

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
                                this.Sheet[e.ColumnIndex, e.RowIndex].Text = newCellText;
                                this.MainForm.spreadsheetViewUI.Rows[e.RowIndex].Cells[e.ColumnIndex].Value =
                                    this.Sheet[e.ColumnIndex, e.RowIndex].Value;
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

        private void InitializeUiGrid()
        {
            char[] alphabet = Enumerable.Range('A', 'Z' - 'A' + 1).Select(i => (char)i).ToArray();
            this.MainForm.spreadsheetViewUI.Columns.Clear();
            foreach (char c in alphabet)
            {
                this.MainForm.spreadsheetViewUI.Columns.Add(c.ToString(), c.ToString());
            }

            this.MainForm.spreadsheetViewUI.Rows.Clear();
            this.MainForm.spreadsheetViewUI.Rows.Add(50);
            for (int rowNumber = 0; rowNumber < 50; rowNumber++)
            {
                this.MainForm.spreadsheetViewUI.Rows[rowNumber].HeaderCell.Value =
                    string.Format($"{this.MainForm.spreadsheetViewUI.Rows[rowNumber].Index + 1}");
            }
        }

        private MainForm MainForm { get; }
    }
}

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
        private string? _mainText;
        private SpreadsheetEngine.Spreadsheet sheet;

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
            this.MainForm.button1.Click += new System.EventHandler(this.Button_Click);
#pragma warning restore CS8622 // Nullability of reference types in type of parameter doesn't match the target delegate (possibly because of nullability attributes).

            this.sheet = new(50, 26);
#pragma warning disable CS8622 // Nullability of reference types in type of parameter doesn't match the target delegate (possibly because of nullability attributes).
            this.sheet.OnCellPropertyChanged += this.UpdateCell;
#pragma warning disable CS8622 // Nullability of reference types in type of parameter doesn't match the target delegate (possibly because of nullability attributes).
            this.MainForm.dataGridView1.Columns.Clear();
            foreach (char c in alphabet)
            {
                this.MainForm.dataGridView1.Columns.Add(c.ToString(), c.ToString());
            }

            this.MainForm.dataGridView1.Rows.Clear();
            this.MainForm.dataGridView1.Rows.Add(50);
            for (int rowNumber = 0; rowNumber < 50; rowNumber++)
            {
                this.MainForm.dataGridView1.Rows[rowNumber].HeaderCell.Value = string.Format($"{this.MainForm.dataGridView1.Rows[rowNumber].Index + 1}");
            }
        }

        /// <summary>
        /// Runs Demo.
        /// </summary>
        /// <param name="sender">sender object.</param>
        /// <param name="e">Property changed event.</param>
        public void Button_Click(object sender, System.EventArgs e)
        {
            this.MainForm.button1.Enabled = false;
            this.MainForm.button1.Hide();
            this.MainForm.button1.Visible = false;
            this.MainForm.button1.UseVisualStyleBackColor = false;
            this.MainForm.button1.SendToBack();
            this.sheet.Demo();
        }

        private void UpdateCell(object sender, PropertyChangedEventArgs e)
        {
            SpreadsheetEngine.SpreadsheetCell temp = sender as SpreadsheetEngine.SpreadsheetCell;
            if (e.PropertyName != null)
            {
                this.MainForm.dataGridView1.Rows[temp!.RowIndex].Cells[temp.ColumnIndex].Value = temp.Value;
            }
        }

        /// <summary>
        /// Gets or sets mainText Property.
        /// </summary>
        public string? MainText
        {
            get
            {
                return this._mainText;
            }

            set
            {
                this._mainText = value;
                this.MainForm.MainTextBox.Text = value;
            }
        }

        private MainForm MainForm { get; }
    }
}

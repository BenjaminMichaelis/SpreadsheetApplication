// <copyright file="MainViewModel.cs" company="Benjamin Michaelis">
// Copyright (c) Benjamin Michaelis. ID: 11620581. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadsheetApp
{
    /// <summary>
    /// ViewModel.
    /// </summary>
    public class MainViewModel
    {
        private string? _mainText;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainViewModel"/> class.
        /// ViewModel - connects Model and View. This layer is an abstraction of the View that exposes public properties and commands used to bind your data to GUI elements and manage this data.
        /// </summary>
        /// <param name="mainForm">pass in the Form to bind.</param>
        public MainViewModel(MainForm mainForm)
        {
            char[] alphabet = Enumerable.Range('A', 'Z' - 'A' + 1).Select(i => (char)i).ToArray();
            this.MainForm = mainForm;
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

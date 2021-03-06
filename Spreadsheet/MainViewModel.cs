// <copyright file="MainViewModel.cs" company="Benjamin Michaelis">
// Copyright (c) Benjamin Michaelis. ID: 11620581. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

            // private Spreadsheet sut = new(26, 50);
            // this.MainForm.dataGridView = Spreadsheet
            // need to subscribe dataGridView1 ui to spreadsheet / spreadsheet events
            // maybe here: https://www.bing.com/search?q=datagridview+instantiate&PC=U316&FORM=CHROMN
            // https://docs.microsoft.com/en-us/dotnet/api/system.windows.forms.datagridview.datasource?view=net-5.0
            // https://docs.microsoft.com/en-us/dotnet/api/system.windows.forms.bindingsource?view=net-5.0
            // https://social.msdn.microsoft.com/Forums/en-US/70138a82-1ab7-4556-af6c-7c31350d091b/new-object-instance-for-datagridview
            this.sheet = new(50, 26);
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

            this.sheet.OnCellPropertyChanged += this.UpdateCell;

            this.MainForm.dataGridView1.Rows[1].Cells[1].Value = "temp";
            // this.sheet.Demo();
        }

        private void UpdateCell(object sender, PropertyChangedEventArgs e)
        {
            SpreadsheetEngine.SpreadsheetCell temp = sender as SpreadsheetEngine.SpreadsheetCell;
            if (e.PropertyName != null)
            {
                this.MainForm.dataGridView1.Rows[temp.RowIndex].Cells[temp.ColumnIndex].Value = temp.Value;
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

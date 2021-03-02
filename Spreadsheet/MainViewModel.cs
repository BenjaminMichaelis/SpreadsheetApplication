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
            this.MainForm = mainForm;
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

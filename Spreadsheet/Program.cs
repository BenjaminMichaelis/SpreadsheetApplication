// <copyright file="Program.cs" company="Benjamin Michaelis">
// Copyright (c) Benjamin Michaelis. ID: 11620581. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

/*
 * WinForms Info:
 * Model - defines the data and your business logic.
 * View - specifies the UI, including all visual elements (buttons, labels, editors, etc.) bound to properties and commands in the ViewModel.
 * ViewModel - connects Model and View. This layer is an abstraction of the View that exposes public properties and commands used to bind your data to GUI elements and manage this data.
 * MVVM Implementation for WinForms: https://www.c-sharpcorner.com/uploadfile/yougerthen/mvvm-implementation-for-windows-forms/
 */

namespace SpreadsheetApp
{
    /// <summary>
    /// Is the main entry point class.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        public static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Initialize();
        }

        private static void Initialize()
        {
            Type type = typeof(MainForm);

            object[] attributes = type.GetCustomAttributes(typeof(ViewModelAttribute), false);

            // if (attributes.Length > 0 && (attributes[0] as ViewModelAttribute).Activated == true)
            MainForm mainForm = new MainForm();
            MainViewModel viewModel = new(mainForm);

            Application.Run(mainForm);
        }
    }
}

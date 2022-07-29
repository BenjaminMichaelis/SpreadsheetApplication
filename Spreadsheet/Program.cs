// <copyright file="Program.cs" company="Benjamin Michaelis">
// Copyright (c) Benjamin Michaelis. All rights reserved.
// </copyright>

using Squirrel;
using System;
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
            SquirrelAwareApp.HandleEvents(
        onInitialInstall: OnAppInstall,
        onAppUninstall: OnAppUninstall,
        onEveryRun: OnAppRun);

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

        private static void OnAppInstall(SemanticVersion version, IAppTools tools)
        {
            tools.CreateShortcutForThisExe(ShortcutLocation.StartMenu | ShortcutLocation.Desktop);
        }

        private static void OnAppUninstall(SemanticVersion version, IAppTools tools)
        {
            tools.RemoveShortcutForThisExe(ShortcutLocation.StartMenu | ShortcutLocation.Desktop);
        }

        private static void OnAppRun(SemanticVersion version, IAppTools tools, bool firstRun)
        {
            tools.SetProcessAppUserModelId();

            // show a welcome message when the app is first installed
            if (firstRun)
            {
                MessageBox.Show("Thanks for installing this Spreadsheet Application!");
            }
        }
    }
}

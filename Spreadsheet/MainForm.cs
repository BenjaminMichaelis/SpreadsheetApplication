// <copyright file="MainForm.cs" company="Benjamin Michaelis">
// Copyright (c) Benjamin Michaelis. All rights reserved.
// </copyright>

using Squirrel;
using System;
using System.Windows.Forms;

namespace SpreadsheetApp
{
    /// <summary>
    /// Form1 class.
    /// </summary>
    public partial class MainForm : Form
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MainForm"/> class.
        /// </summary>
        public MainForm()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Run Form1_Load.
        /// </summary>
        /// <param name="sender">First Param.</param>
        /// <param name="e">Second Param.</param>
        public void Form1_Load(object sender, EventArgs e)
        {
        }

        private async void MainForm_Load(object sender, EventArgs e)
        {
            using var mgr = new UpdateManager(urlOrPath: null);
            if (mgr.IsInstalledApp)
            {
                const string channel = "production";
                using var remoteManager = new UpdateManager($"https://localhost:7155/Squirrel/{mgr.AppId}/{channel}");
                var newVersion = await remoteManager.UpdateApp();

                // optionally restart the app automatically, or ask the user if/when they want to restart
                if (newVersion != null)
                {
                    UpdateManager.RestartApp();
                }
            }
        }
    }
}

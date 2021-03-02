using System;
using System.Windows.Forms;

namespace SpreadsheetApp
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // MainTextBox
            // 
            this.MainTextBox.AccessibleDescription = "";
            this.MainTextBox.AccessibleName = "";
            this.MainTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainTextBox.Location = new System.Drawing.Point(0, 0);
            this.MainTextBox.Multiline = true;
            this.MainTextBox.Name = "MainTextBox";
            this.MainTextBox.Size = new System.Drawing.Size(800, 450);
            this.MainTextBox.TabIndex = 0;
            this.MainTextBox.WordWrap = true;
            this.MainTextBox.ReadOnly = true;
            this.MainTextBox.ScrollBars = ScrollBars.Both;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.MainTextBox);
            this.Name = "Benjamin Michaelis - 11620581";
            this.Name = "MainForm";
            this.Text = "Benjamin Michaelis - 11620581";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        /// <summary>
        /// MainTextBox.
        /// </summary>
        public System.Windows.Forms.TextBox MainTextBox { get; } = new System.Windows.Forms.TextBox();
    }
}

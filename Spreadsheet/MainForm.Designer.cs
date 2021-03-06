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
            // mainmenu
            // 
            this.mainmenu.AllowDrop = true;
            this.mainmenu.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.mainmenu.Location = new System.Drawing.Point(0, 0);
            this.mainmenu.Name = "mainmenu";
            this.mainmenu.Size = new System.Drawing.Size(800, 28);
            this.mainmenu.TabIndex = 0;
            this.mainmenu.Text = "Main Menu";
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(13, 13);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersWidth = 51;
            this.dataGridView1.RowTemplate.Height = 29;
            this.dataGridView1.Size = new System.Drawing.Size(750, 400);
            this.dataGridView1.TabIndex = 0;
            //// 
            //// button1
            //// 
            this.button1.Location = new System.Drawing.Point(0, 61);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(400, 200);
            this.button1.TabIndex = 1;
            this.button1.Text = "Perform Demo";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.dataGridView1);
            this.MainMenuStrip = this.mainmenu;
            this.Name = "MainForm";
            this.Text = "Benjamin Michaelis - 11620581";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        /// <summary>
        /// gets dataGridView for MainViewModel.
        /// </summary>
        public DataGridView dataGridView1 { get; } = new();
        /// <summary>
        /// Button for running demo.
        /// </summary>
        public System.Windows.Forms.Button button1 { get; } = new();
        /// <summary>
        /// Creates menustrip.
        /// </summary>
        public MenuStrip mainmenu { get; } = new();

        #endregion

        /// <summary>
        /// MainTextBox.
        /// </summary>
        public System.Windows.Forms.TextBox MainTextBox { get; } = new System.Windows.Forms.TextBox();
    }
}

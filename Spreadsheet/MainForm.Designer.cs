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
            // mainMenu
            // 
            this.mainMenu.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.mainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
                this.FileButton, this.EditButton, this.CellButton});
            this.mainMenu.Location = new System.Drawing.Point(0, 0);
            this.mainMenu.Name = "mainMenu";
            this.mainMenu.Size = new System.Drawing.Size(800, 28);
            this.mainMenu.TabIndex = 0;
            this.mainMenu.Text = "Main Menu";
            this.mainMenu.Anchor = (AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top);
            // 
            // EditButton
            // 
            this.EditButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
                this.UndoButton, this.RedoButton});
            this.EditButton.Name = "EditButton";
            this.EditButton.Size = new System.Drawing.Size(48, 24);
            this.EditButton.Text = "Edit";
            // 
            // UndoButton
            // 
            this.UndoButton.Name = "UndoButton";
            this.UndoButton.Size = new System.Drawing.Size(274, 26);
            this.UndoButton.Text = "Undo";
            // 
            // RedoButton
            // 
            this.RedoButton.Name = "RedoButton";
            this.RedoButton.Size = new System.Drawing.Size(274, 26);
            this.RedoButton.Text = "Redo";
            // 
            // FileButton
            // 
            this.FileButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
                this.DemoButton});
            this.FileButton.Name = "FileButton";
            this.FileButton.Size = new System.Drawing.Size(48, 24);
            this.FileButton.Text = "File";
            // 
            // DemoButton
            // 
            this.DemoButton.Name = "DemoButton";
            this.DemoButton.Size = new System.Drawing.Size(274, 26);
            this.DemoButton.Text = "Demo";
            // 
            // CellButton
            // 
            this.CellButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.changeBackgroundColorButton});
            this.CellButton.Name = "CellButton";
            this.CellButton.Size = new System.Drawing.Size(48, 24);
            this.CellButton.Text = "Cell";
            // 
            // changeBackgroundColorButton
            // 
            this.changeBackgroundColorButton.Name = "changeBackgroundColorButton";
            this.changeBackgroundColorButton.Size = new System.Drawing.Size(274, 26);
            this.changeBackgroundColorButton.Text = "Change Background Color...";
            // 
            // spreadsheetViewUI
            // 
            this.spreadsheetViewUI.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.spreadsheetViewUI.Location = new System.Drawing.Point(13, 30);
            this.spreadsheetViewUI.Name = "spreadsheetViewUI";
            this.spreadsheetViewUI.RowHeadersWidth = 51;
            this.spreadsheetViewUI.RowTemplate.Height = 29;
            this.spreadsheetViewUI.Size = new System.Drawing.Size(750, 400);
            this.spreadsheetViewUI.TabIndex = 0;
            this.spreadsheetViewUI.Anchor = (AnchorStyles.Bottom | AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.spreadsheetViewUI);
            this.Controls.Add(this.mainMenu);
            this.MainMenuStrip = this.mainMenu;
            this.Name = "MainForm";
            this.Text = "Benjamin Michaelis - 11620581";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        /// <summary>
        /// Cell Dropdown Button.
        /// </summary>
        public ToolStripMenuItem CellButton { get; } = new();

        /// <summary>
        /// File Dropdown Button.
        /// </summary>
        public ToolStripMenuItem FileButton { get; } = new();

        /// <summary>
        /// Edit Dropdown Button.
        /// </summary>
        public ToolStripMenuItem EditButton { get; } = new();

        /// <summary>
        /// Change Background Color Button.
        /// </summary>
        public ToolStripMenuItem changeBackgroundColorButton { get; } = new();

        /// <summary>
        /// Demo Button.
        /// </summary>
        public ToolStripMenuItem DemoButton { get; } = new();

        /// <summary>
        /// Undo Button.
        /// </summary>
        public ToolStripMenuItem UndoButton { get; } = new();

        /// <summary>
        /// Redo Button.
        /// </summary>
        public ToolStripMenuItem RedoButton { get; } = new();


        /// <summary>
        /// gets dataGridView for MainViewModel.
        /// </summary>
        public DataGridView spreadsheetViewUI { get; } = new();
        /// <summary>
        /// Creates menustrip.
        /// </summary>
        public MenuStrip mainMenu { get; } = new();

        /// <summary>
        /// MainTextBox.
        /// </summary>
        public System.Windows.Forms.TextBox MainTextBox { get; } = new System.Windows.Forms.TextBox();

        #endregion
    }
}

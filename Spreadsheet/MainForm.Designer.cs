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
            this.A = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.B = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.C = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.D = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.E = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.F = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.G = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.A,
            this.B,
            this.C,
            this.D,
            this.E,
            this.F,
            this.G});
            this.dataGridView1.Location = new System.Drawing.Point(13, 13);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersWidth = 51;
            this.dataGridView1.RowTemplate.Height = 29;
            this.dataGridView1.Size = new System.Drawing.Size(750, 400);
            this.dataGridView1.TabIndex = 0;
            // 
            // A
            // 
            this.A.HeaderText = "A";
            this.A.Name = "A";
            this.A.Width = 125;
            // 
            // B
            // 
            this.B.HeaderText = "B";
            this.B.Name = "B";
            this.B.Width = 125;
            // 
            // C
            // 
            this.C.HeaderText = "C";
            this.C.Name = "C";
            this.C.Width = 125;
            // 
            // D
            // 
            this.D.HeaderText = "D";
            this.D.Name = "D";
            this.D.Width = 125;
            // 
            // E
            // 
            this.E.HeaderText = "E";
            this.E.Name = "E";
            this.E.Width = 125;
            // 
            // F
            // 
            this.F.HeaderText = "F";
            this.F.Name = "F";
            this.F.Width = 125;
            // 
            // G
            // 
            this.G.HeaderText = "G";
            this.G.Name = "G";
            this.G.Width = 125;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.dataGridView1);
            this.Name = "MainForm";
            this.Text = "Benjamin Michaelis - 11620581";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        /// <summary>
        /// gets dataGridView for MainViewModel.
        /// </summary>
        public DataGridView dataGridView1 { get; } = new();
        private DataGridViewTextBoxColumn A;
        private DataGridViewTextBoxColumn B;
        private DataGridViewTextBoxColumn C;
        private DataGridViewTextBoxColumn D;
        private DataGridViewTextBoxColumn E;
        private DataGridViewTextBoxColumn F;
        private DataGridViewTextBoxColumn G;

        #endregion

        /// <summary>
        /// MainTextBox.
        /// </summary>
        public System.Windows.Forms.TextBox MainTextBox { get; } = new System.Windows.Forms.TextBox();
    }
}

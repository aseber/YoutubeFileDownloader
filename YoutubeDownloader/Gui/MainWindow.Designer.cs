﻿namespace YoutubeDownloader.Gui
{
    partial class MainWindow
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
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.downloadsList = new System.Windows.Forms.CheckedListBox();
            this.workingDirectoryButton = new System.Windows.Forms.Button();
            this.workingDirectoryTextBox = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.BackColor = System.Drawing.SystemColors.Control;
            this.tableLayoutPanel.ColumnCount = 2;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 91.89744F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 8.102564F));
            this.tableLayoutPanel.Controls.Add(this.downloadsList, 0, 1);
            this.tableLayoutPanel.Controls.Add(this.workingDirectoryButton, 1, 0);
            this.tableLayoutPanel.Controls.Add(this.workingDirectoryTextBox, 0, 0);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.GrowStyle = System.Windows.Forms.TableLayoutPanelGrowStyle.FixedSize;
            this.tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel.Margin = new System.Windows.Forms.Padding(2);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 2;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.48936F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 88.51064F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 39F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(487, 275);
            this.tableLayoutPanel.TabIndex = 5;
            // 
            // downloadsList
            // 
            this.downloadsList.AllowDrop = true;
            this.downloadsList.CheckOnClick = true;
            this.tableLayoutPanel.SetColumnSpan(this.downloadsList, 2);
            this.downloadsList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.downloadsList.FormattingEnabled = true;
            this.downloadsList.Location = new System.Drawing.Point(3, 34);
            this.downloadsList.Name = "downloadsList";
            this.downloadsList.Size = new System.Drawing.Size(481, 238);
            this.downloadsList.TabIndex = 0;
            this.downloadsList.TabStop = false;
            this.downloadsList.DragDrop += new System.Windows.Forms.DragEventHandler(this.UrlDropped);
            this.downloadsList.DragEnter += new System.Windows.Forms.DragEventHandler(this.ValidateDragContent);
            // 
            // workingDirectoryButton
            // 
            this.workingDirectoryButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.workingDirectoryButton.Location = new System.Drawing.Point(450, 3);
            this.workingDirectoryButton.Name = "workingDirectoryButton";
            this.workingDirectoryButton.Size = new System.Drawing.Size(34, 25);
            this.workingDirectoryButton.TabIndex = 1;
            this.workingDirectoryButton.Text = "...";
            this.workingDirectoryButton.UseVisualStyleBackColor = true;
            this.workingDirectoryButton.Click += new System.EventHandler(this.ChangeWorkingDirectory);
            // 
            // workingDirectoryTextBox
            // 
            this.workingDirectoryTextBox.Cursor = System.Windows.Forms.Cursors.Default;
            this.workingDirectoryTextBox.Location = new System.Drawing.Point(3, 3);
            this.workingDirectoryTextBox.MaxLength = 260;
            this.workingDirectoryTextBox.Name = "workingDirectoryTextBox";
            this.workingDirectoryTextBox.ReadOnly = true;
            this.workingDirectoryTextBox.Size = new System.Drawing.Size(441, 20);
            this.workingDirectoryTextBox.TabIndex = 2;
            this.workingDirectoryTextBox.TabStop = false;
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(487, 275);
            this.Controls.Add(this.tableLayoutPanel);
            this.MaximizeBox = false;
            this.Name = "MainWindow";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Youtube Downloader";
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.CheckedListBox downloadsList;
        private System.Windows.Forms.Button workingDirectoryButton;
        private System.Windows.Forms.TextBox workingDirectoryTextBox;
    }
}
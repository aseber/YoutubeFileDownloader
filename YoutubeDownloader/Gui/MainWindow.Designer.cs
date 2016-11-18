namespace YouTubeDownloader.Gui
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.workingDirectoryButton = new System.Windows.Forms.Button();
            this.workingDirectoryTextBox = new System.Windows.Forms.TextBox();
            this.downloadsList = new System.Windows.Forms.ListBox();
            this.tableLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.tableLayoutPanel.BackColor = System.Drawing.SystemColors.Control;
            this.tableLayoutPanel.ColumnCount = 2;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 91.89744F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 8.102564F));
            this.tableLayoutPanel.Controls.Add(this.workingDirectoryButton, 1, 0);
            this.tableLayoutPanel.Controls.Add(this.workingDirectoryTextBox, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.downloadsList, 0, 1);
            this.tableLayoutPanel.GrowStyle = System.Windows.Forms.TableLayoutPanelGrowStyle.FixedSize;
            this.tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel.Margin = new System.Windows.Forms.Padding(2);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 2;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10.54545F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 89.45454F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 39F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(487, 275);
            this.tableLayoutPanel.TabIndex = 5;
            // 
            // workingDirectoryButton
            // 
            this.workingDirectoryButton.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.workingDirectoryButton.Location = new System.Drawing.Point(450, 3);
            this.workingDirectoryButton.Name = "workingDirectoryButton";
            this.workingDirectoryButton.Size = new System.Drawing.Size(34, 22);
            this.workingDirectoryButton.TabIndex = 1;
            this.workingDirectoryButton.Text = "...";
            this.workingDirectoryButton.UseVisualStyleBackColor = true;
            this.workingDirectoryButton.Click += new System.EventHandler(this.ChangeWorkingDirectory);
            // 
            // workingDirectoryTextBox
            // 
            this.workingDirectoryTextBox.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.workingDirectoryTextBox.Cursor = System.Windows.Forms.Cursors.Default;
            this.workingDirectoryTextBox.Location = new System.Drawing.Point(3, 4);
            this.workingDirectoryTextBox.MaxLength = 260;
            this.workingDirectoryTextBox.Name = "workingDirectoryTextBox";
            this.workingDirectoryTextBox.ReadOnly = true;
            this.workingDirectoryTextBox.Size = new System.Drawing.Size(441, 20);
            this.workingDirectoryTextBox.TabIndex = 2;
            this.workingDirectoryTextBox.TabStop = false;
            // 
            // downloadsList
            // 
            this.downloadsList.AllowDrop = true;
            this.downloadsList.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.tableLayoutPanel.SetColumnSpan(this.downloadsList, 2);
            this.downloadsList.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.downloadsList.FormattingEnabled = true;
            this.downloadsList.Location = new System.Drawing.Point(3, 31);
            this.downloadsList.Name = "downloadsList";
            this.downloadsList.SelectionMode = System.Windows.Forms.SelectionMode.None;
            this.downloadsList.Size = new System.Drawing.Size(481, 240);
            this.downloadsList.TabIndex = 3;
            this.downloadsList.UseTabStops = false;
            this.downloadsList.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.GetDrawItemDelegate);
            this.downloadsList.DragDrop += new System.Windows.Forms.DragEventHandler(this.UrlDropped);
            this.downloadsList.DragEnter += new System.Windows.Forms.DragEventHandler(this.ValidateDragContent);
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(487, 275);
            this.Controls.Add(this.tableLayoutPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainWindow";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "YouTube Downloader";
            this.TopMost = true;
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.Button workingDirectoryButton;
        private System.Windows.Forms.TextBox workingDirectoryTextBox;
        private System.Windows.Forms.ListBox downloadsList;
    }
}
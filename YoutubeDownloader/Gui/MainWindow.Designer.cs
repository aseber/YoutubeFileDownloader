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
            this.tableLayoutPanel.AutoSize = true;
            this.tableLayoutPanel.BackColor = System.Drawing.SystemColors.Control;
            this.tableLayoutPanel.ColumnCount = 2;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tableLayoutPanel.Controls.Add(this.workingDirectoryButton, 1, 0);
            this.tableLayoutPanel.Controls.Add(this.workingDirectoryTextBox, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.downloadsList, 0, 1);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.GrowStyle = System.Windows.Forms.TableLayoutPanelGrowStyle.FixedSize;
            this.tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel.Margin = new System.Windows.Forms.Padding(4);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 2;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.Size = new System.Drawing.Size(974, 529);
            this.tableLayoutPanel.TabIndex = 5;
            // 
            // workingDirectoryButton
            // 
            this.workingDirectoryButton.AutoSize = true;
            this.workingDirectoryButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.workingDirectoryButton.Location = new System.Drawing.Point(920, 6);
            this.workingDirectoryButton.Margin = new System.Windows.Forms.Padding(6);
            this.workingDirectoryButton.Name = "workingDirectoryButton";
            this.workingDirectoryButton.Size = new System.Drawing.Size(48, 28);
            this.workingDirectoryButton.TabIndex = 1;
            this.workingDirectoryButton.Text = "...";
            this.workingDirectoryButton.UseVisualStyleBackColor = true;
            this.workingDirectoryButton.Click += new System.EventHandler(this.ChangeWorkingDirectory);
            // 
            // workingDirectoryTextBox
            // 
            this.workingDirectoryTextBox.Cursor = System.Windows.Forms.Cursors.Default;
            this.workingDirectoryTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.workingDirectoryTextBox.Location = new System.Drawing.Point(6, 6);
            this.workingDirectoryTextBox.Margin = new System.Windows.Forms.Padding(6);
            this.workingDirectoryTextBox.MaxLength = 260;
            this.workingDirectoryTextBox.Name = "workingDirectoryTextBox";
            this.workingDirectoryTextBox.ReadOnly = true;
            this.workingDirectoryTextBox.Size = new System.Drawing.Size(902, 31);
            this.workingDirectoryTextBox.TabIndex = 2;
            this.workingDirectoryTextBox.TabStop = false;
            // 
            // downloadsList
            // 
            this.downloadsList.AllowDrop = true;
            this.tableLayoutPanel.SetColumnSpan(this.downloadsList, 2);
            this.downloadsList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.downloadsList.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.downloadsList.FormattingEnabled = true;
            this.downloadsList.Location = new System.Drawing.Point(6, 46);
            this.downloadsList.Margin = new System.Windows.Forms.Padding(6);
            this.downloadsList.Name = "downloadsList";
            this.downloadsList.SelectionMode = System.Windows.Forms.SelectionMode.None;
            this.downloadsList.Size = new System.Drawing.Size(962, 477);
            this.downloadsList.TabIndex = 3;
            this.downloadsList.UseTabStops = false;
            this.downloadsList.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.GetDrawItemDelegate);
            this.downloadsList.DragDrop += new System.Windows.Forms.DragEventHandler(this.UrlDropped);
            this.downloadsList.DragEnter += new System.Windows.Forms.DragEventHandler(this.ValidateDragContent);
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(974, 529);
            this.Controls.Add(this.tableLayoutPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(6);
            this.MinimumSize = new System.Drawing.Size(1000, 600);
            this.Name = "MainWindow";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "YouTube Downloader";
            this.TopMost = true;
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.Button workingDirectoryButton;
        private System.Windows.Forms.TextBox workingDirectoryTextBox;
        private System.Windows.Forms.ListBox downloadsList;
    }
}
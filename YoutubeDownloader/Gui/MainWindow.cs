using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace YoutubeDownloader.Gui
{
    public partial class MainWindow : Form
    {
        private FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
        private string rootDirectory;

        public MainWindow(string workingDirectory)
        {
            InitializeComponent();
            folderBrowserDialog.Description = "Select the root directory for your git repositories";
            folderBrowserDialog.ShowNewFolderButton = false;
            SetWorkingDirectory(workingDirectory);
        }

        private void AddUrlForDownload(string url)
        {
            downloadsList.Items.Add(url);
        }

        public void SetWorkingDirectory(string workingDirectory)
        {
            Start.SetWorkingDirectory(workingDirectory);
            rootDirectory = workingDirectory;
            workingDirectoryTextBox.Text = workingDirectory;
        }

        private void ChangeWorkingDirectory(object sender, EventArgs e)
        {
            folderBrowserDialog.RootFolder = Environment.SpecialFolder.MyComputer;
            var dialogResult = folderBrowserDialog.ShowDialog();

            if (dialogResult == DialogResult.OK)
            {
                SetWorkingDirectory(folderBrowserDialog.SelectedPath);
            }
        }

        private void UrlDropped(object sender, DragEventArgs e)
        {
            AddUrlForDownload(e.Data.GetData(DataFormats.Text).ToString());
        }

        private void ValidateDragContent(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.Text))
            {
                e.Effect = DragDropEffects.Copy;

            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using YouTubeFileDownloaderApi;
using System.Drawing;

namespace YouTubeDownloader.Gui
{
    public partial class MainWindow : Form
    {

        private static readonly Dictionary<DownloadableFile.DownloadStatus, Color> StatusColorDictionary = new Dictionary<DownloadableFile.DownloadStatus, Color>()
        {
            {DownloadableFile.DownloadStatus.Waiting, Color.White },
            {DownloadableFile.DownloadStatus.Downloading, Color.Yellow },
            {DownloadableFile.DownloadStatus.Converting, Color.Yellow },
            {DownloadableFile.DownloadStatus.Failed, Color.Red },
            {DownloadableFile.DownloadStatus.Completed, Color.Green },
        };

        private FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
        private YoutubeDownloader downloader;
        private string workingDirectory;
        private BindingList<DownloadableFile> fileBinding = new BindingList<DownloadableFile>();

        public MainWindow(string workingDirectory)
        {
            InitializeComponent();
            folderBrowserDialog.Description = "Select the directory that you want to save files to";
            folderBrowserDialog.ShowNewFolderButton = false;
            downloader = new YoutubeDownloader(workingDirectory);
            SetWorkingDirectory(workingDirectory);
            downloadsList.DataSource = fileBinding;
        }

        private void GetDrawItemDelegate(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground();
            Graphics g = e.Graphics;

            var file = downloadsList.Items[e.Index] as DownloadableFile;

            Color color;
            StatusColorDictionary.TryGetValue(file.status, out color);

            var itemHeight = downloadsList.ItemHeight;
            var textDistance = itemHeight + 2;

            var bgBounds = e.Bounds;
            bgBounds.Width = itemHeight;

            var textBounds = e.Bounds;
            textBounds.X = textBounds.X + textDistance;
            textBounds.Width = textBounds.Width - textDistance;

            g.FillRectangle(new SolidBrush(color), bgBounds);
            g.DrawRectangle(Pens.Black, bgBounds);
            g.DrawString(file.ToString(), e.Font, Brushes.Black, textBounds);

            e.DrawFocusRectangle();
        }

        private void AddUrlForDownload(string url)
        {
            DownloadableFile file;

            try
            {
                file = new DownloadableFile(url, DownloadableFile.Audio);
            }
            catch (SystemException)
            {
                MessageBox.Show("Are you sure the video resided on YouTube?", "Error downloading file", MessageBoxButtons.OK);
                return;
            }

            fileBinding.Add(file);
            downloader.DownloadAsync(file);
        }

        public void SetWorkingDirectory(string workingDirectory)
        {
            Start.SetWorkingDirectory(workingDirectory);
            this.workingDirectory = workingDirectory;
            workingDirectoryTextBox.Text = workingDirectory;
            downloader.downloadPath = workingDirectory;
        }

        private void ChangeWorkingDirectory(object sender, EventArgs e)
        {
            folderBrowserDialog.RootFolder = Environment.SpecialFolder.MyComputer;
            folderBrowserDialog.SelectedPath = workingDirectory;
            var dialogResult = folderBrowserDialog.ShowDialog();

            if (dialogResult == DialogResult.OK)
            {
                SetWorkingDirectory(folderBrowserDialog.SelectedPath + "\\");
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

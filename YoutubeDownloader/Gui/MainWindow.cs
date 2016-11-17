using System;
using System.ComponentModel;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using YoutubeFileDownloaderApi;

namespace YoutubeDownloaderGui.Gui
{
    public partial class MainWindow : Form
    {
        private FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
        private YoutubeDownloader downloader;
        private string workingDirectory;
        private BindingList<DownloadableFile> fileBinding = new BindingList<DownloadableFile>();

        public MainWindow(string workingDirectory)
        {
            InitializeComponent();
            folderBrowserDialog.Description = "Select the directory that you want to save files to";
            folderBrowserDialog.ShowNewFolderButton = false;
            SetWorkingDirectory(workingDirectory);
            downloadsList.DataSource = fileBinding;
        }

        private void AddUrlForDownload(string url)
        {
            DownloadableFile file = new DownloadableFile(url, DownloadableFile.Audio);
            fileBinding.Add(file);
            downloader.DownloadAsync(file);
        }

        public void SetWorkingDirectory(string workingDirectory)
        {
            Start.SetWorkingDirectory(workingDirectory);
            this.workingDirectory = workingDirectory;
            workingDirectoryTextBox.Text = workingDirectory;
            downloader = new YoutubeDownloader(workingDirectory);
        }

        private void ChangeWorkingDirectory(object sender, EventArgs e)
        {
            folderBrowserDialog.RootFolder = Environment.SpecialFolder.MyComputer;
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

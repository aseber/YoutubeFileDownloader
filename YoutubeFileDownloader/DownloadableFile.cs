using NReco.VideoConverter;
using System;
using System.ComponentModel;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using VideoLibrary;

namespace YoutubeFileDownloaderApi
{
    public class DownloadableFile : INotifyPropertyChanged
    {
        public delegate Task SavingHandler(DownloadableFile file, string downloadPath);
        //public delegate void StatusChangeCallback(DownloadableFile file);

        public static readonly SavingHandler Audio = async (DownloadableFile file, string downloadPath) =>
        {
            var videoName = file.videoName;

            string videoFileName = downloadPath + videoName + ".data";
            string audioFileName = downloadPath + videoName + ".mp3";

            file.status = DownloadStatus.Downloading;

            File.WriteAllBytes(videoFileName, await file.videoHandle.GetBytesAsync());

            /*
             
            // I would like to use this method, but StreamAsync currently doesn't work.

            var videoStream = await file.videoHandle.StreamAsync();
            var fileStream = new FileStream(videoFileName, FileMode.Create, FileAccess.Write);
            await videoStream.CopyToAsync(fileStream);
             
             */

            file.status = DownloadStatus.Converting;

            new FFMpegConverter().ConvertMedia(videoFileName, audioFileName, "mp3");
            File.Delete(videoFileName);

            file.status = DownloadStatus.Finished;
        };

        public static readonly SavingHandler Video = async (DownloadableFile file, string downloadPath) =>
        {
            var videoName = file.videoName;
            var videoExtension = file.videoHandle.FileExtension;

            string videoFileName = downloadPath + videoName + videoExtension;

            file.status = DownloadStatus.Downloading;

            File.WriteAllBytes(videoFileName, await file.videoHandle.GetBytesAsync());

            /*
             
            // I would like to use this method, but StreamAsync currently doesn't work.

            var videoStream = await file.videoHandle.StreamAsync();
            var fileStream = new FileStream(videoFileName, FileMode.Create, FileAccess.Write);
            await videoStream.CopyToAsync(fileStream);
             
             */

            file.status = DownloadStatus.Finished;
        };

        public enum DownloadStatus
        {
            Waiting,
            Downloading,
            Converting,
            Finished,
            Failed
        }

        private const string urlRequirement = "www.youtube.com";

        public event PropertyChangedEventHandler PropertyChanged;

        internal readonly string videoName;
        internal readonly string fileUrl;
        internal readonly SavingHandler handleDownload;
        internal readonly YouTubeVideo videoHandle;
        private DownloadStatus m_status;
        public DownloadStatus status {
            get
            {
                return m_status;
            }
            internal set
            {
                m_status = value;
                NotifyPropertyChanged("status");
            }
        }

        public DownloadableFile(string fileUrl, SavingHandler downloadType)
        {
            if (!fileUrl.Contains(urlRequirement))
            {
                throw new SystemException("In order to download the file, it must come from YouTube");
            }

            videoHandle = YouTube.Default.GetVideo(fileUrl);

            videoName = GetVideoName(videoHandle);
            this.fileUrl = fileUrl;
            handleDownload = downloadType;
            status = DownloadStatus.Waiting;
        }

        public override string ToString()
        {
            return videoName + " (" + status + ")";
        }

        private string GetVideoName(YouTubeVideo video)
        {
            string fullVideoName = video.Title;

            // Add more to this to clean up the file name!

            return fullVideoName;
        }

        private void NotifyPropertyChanged(string info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }
    }
}

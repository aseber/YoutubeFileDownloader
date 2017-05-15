using System;
using System.ComponentModel;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using YoutubeExplode;
using YoutubeExplode.Models;
using YoutubeExplode.Models.MediaStreams;
using static YouTubeFileDownloaderApi.AsyncSaveHandlerFactory;

namespace YouTubeFileDownloaderApi
{
    public class DownloadableFile : INotifyPropertyChanged
    {
        public enum DownloadStatus
        {
            Waiting,
            Downloading,
            Converting,
            Completed,
            Failed
        }

        private readonly static YoutubeClient youtubeClient = new YoutubeClient();

        public event PropertyChangedEventHandler PropertyChanged;

        internal readonly AsyncSaveHandler DoAsyncSave;
        internal readonly string videoId;

        private VideoInfo m_videoHandle;
        internal VideoInfo videoHandle {
            get
            {
                if (m_videoHandle == null)
                {
                    m_videoHandle = youtubeClient.GetVideoInfoAsync(videoId).Result;
                    NotifyPropertyChanged("status");
                }

                return m_videoHandle;
            }
            private set
            {
                m_videoHandle = value;
            }
        }
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

        public DownloadableFile(string url, AsyncSaveHandler downloadType)
        {
            var videoId = string.Empty;

            if (!YoutubeClient.TryParseVideoId(url, out videoId) | !youtubeClient.CheckVideoExistsAsync(videoId).Result)
            {
                throw new SystemException("In order to download the file, it must come from YouTube");
            }

            this.videoId = videoId;
            DoAsyncSave = downloadType;
            status = DownloadStatus.Waiting;
        }

        public Task<MediaStream> GetStreamAsync(MediaStreamInfo info)
        {
            return youtubeClient.GetMediaStreamAsync(info);
        }

        public override string ToString()
        {
            return GetVideoName() + " (" + status + ")";
        }

        public string GetVideoName()
        {
            if (m_videoHandle == null)
            {
                return "<unknown title>";
            }

            string fullVideoName = videoHandle.Title;

            fullVideoName = string.Join("�", fullVideoName.Split(Path.GetInvalidFileNameChars()));
            fullVideoName = string.Join("�", fullVideoName.Split(Path.GetInvalidPathChars()));

            return fullVideoName;
        }

        private void NotifyPropertyChanged(string info)
        {
            if (PropertyChanged != null)
            {
                if (Application.OpenForms.Count > 0)
                {
                    var form = Application.OpenForms[0]; // Need to fix this
                    form.Invoke(new Action(() => PropertyChanged(this, new PropertyChangedEventArgs(info))));
                }
            }
        }
    }
}

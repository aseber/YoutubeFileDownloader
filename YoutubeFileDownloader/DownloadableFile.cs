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
        public delegate Task SavingHandler(DownloadableFile file, Stream videoStream, string downloadPath);

        public static readonly SavingHandler Audio = async (DownloadableFile file, Stream videoStream, string downloadPath) =>
        {
            var videoName = file.videoName;

            string videoFileName = downloadPath + videoName + ".data";
            string audioFileName = downloadPath + videoName + ".mp3";

            file.status = DownloadStatus.Downloading;

            using (var fileStream = new FileStream(videoFileName, FileMode.Create, FileAccess.Write))
            {
                await videoStream.CopyToAsync(fileStream);
            }

            file.status = DownloadStatus.Converting;
            await Task.Factory.StartNew(() => { new FFMpegConverter().ConvertMedia(videoFileName, audioFileName, "mp3"); } );
            File.Delete(videoFileName);

            file.status = DownloadStatus.Completed;
        };

        public static readonly SavingHandler Video = async (DownloadableFile file, Stream videoStream, string downloadPath) =>
        {
            var videoName = file.videoName;
            var videoExtension = file.videoHandle.FileExtension;

            string videoFileName = downloadPath + videoName + videoExtension;

            file.status = DownloadStatus.Downloading;

            using (var fileStream = new FileStream(videoFileName, FileMode.Create, FileAccess.Write))
            {
                await videoStream.CopyToAsync(fileStream);
            }

            file.status = DownloadStatus.Completed;
        };

        public enum DownloadStatus
        {
            Waiting,
            Downloading,
            Converting,
            Completed,
            Failed
        }

        private const string urlRequirement = "www.youtube.com";

        public event PropertyChangedEventHandler PropertyChanged;

        internal readonly string videoName;
        internal readonly string fileUrl;
        internal readonly SavingHandler DoSave;
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
            DoSave = downloadType;
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
                if (Application.OpenForms.Count > 0)
                {
                    var form = Application.OpenForms[0];
                    form.Invoke(new Action(() => PropertyChanged(this, new PropertyChangedEventArgs(info))));
                }

                /*var context = SynchronizationContext.Current ?? new SynchronizationContext();

                                context.Send(s => PropertyChanged(this, new PropertyChangedEventArgs(info)), null);

                                var asyncOp = AsyncOperationManager.CreateOperation(null);
                                asyncOp.Post(() => PropertyChanged(this, new PropertyChangedEventArgs("")), PropertyChanged.Target);

                                /*() => { PropertyChanged(this, new PropertyChangedEventArgs("")); }*/

                //((BindingList<DownloadableFile>) PropertyChanged.Target)./*something here to get the form*/.Invoke(new Action(() => PropertyChanged(this, new PropertyChangedEventArgs(""))));
            }
        }
    }
}

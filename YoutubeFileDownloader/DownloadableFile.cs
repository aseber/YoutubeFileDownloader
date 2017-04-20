using NReco.VideoConverter;
using System;
using System.ComponentModel;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using VideoLibrary;
using static YouTubeFileDownloaderApi.SaveHandlerFactory;

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

        private const string urlRequirement = "www.youtube.com";

        public event PropertyChangedEventHandler PropertyChanged;

        internal readonly string videoName;
        internal readonly string fileUrl;
        internal readonly SaveHandler DoSave;
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

        public DownloadableFile(string fileUrl, SaveHandler downloadType)
        {
            if (!fileUrl.Contains(urlRequirement))
            {
                throw new SystemException("In order to download the file, it must come from YouTube");
            }

            Console.WriteLine(fileUrl);

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

            fullVideoName = string.Join("�", fullVideoName.Split(Path.GetInvalidFileNameChars()));
            fullVideoName = string.Join("�", fullVideoName.Split(Path.GetInvalidPathChars()));
            // Add more to this to clean up the file name!

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

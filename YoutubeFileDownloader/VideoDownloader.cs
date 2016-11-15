using NReco.VideoConverter;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VideoLibrary;

namespace YoutubeFileDownloaderApi
{
    class YoutubeDownloader
    {
        public delegate Task SavingHandler(DownloadableFile file, YouTubeVideo video, string downloadPath);
        public class DownloadableFile
        {
            public enum DownloadStatus {
                Waiting,
                Downloading,
                Finished,
                Failed
            }

            internal string fileName { get; }
            internal string fileUrl { get; }
            internal SavingHandler downloadType { get; }
            public DownloadStatus status { get; internal set; }

            public DownloadableFile(string fileName, string fileUrl, SavingHandler downloadType)
            {
                this.fileName = fileName;
                this.fileUrl = fileUrl;
                this.downloadType = downloadType;
                this.status = DownloadStatus.Waiting;
            }
        }

        public static readonly SavingHandler Audio = async (DownloadableFile file, YouTubeVideo video, string downloadPath) =>
        {
            var videoName = file.fileName;
            var fullVideoName = video.FullName;

            File.WriteAllBytes(downloadPath + fullVideoName, await video.GetBytesAsync());
            new FFMpegConverter().ConvertMedia(downloadPath + fullVideoName, downloadPath + videoName + ".mp3", "mp3");
            File.Delete(downloadPath + fullVideoName);
        };

        public static readonly SavingHandler Video = async (DownloadableFile file, YouTubeVideo video, string downloadPath) =>
        {
            var videoName = file.fileName;
            var videoExtension = video.FileExtension;

            File.WriteAllBytes(downloadPath + videoName + videoExtension, await video.GetBytesAsync());
        };

        private readonly string downloadPath;
        private int downloadIndex = 0;

        public YoutubeDownloader(string downloadPath)
        {
            this.downloadPath = downloadPath;
        }

        public void DownloadAsync(IEnumerable<DownloadableFile> files, int concurrentDownloads = 4)
        {
            var semaphore = new SemaphoreSlim(concurrentDownloads);

            var tasks = files.Select(async file =>
            {
                await semaphore.WaitAsync();

                try
                {
                    await DownloadAsync(file);
                }
                finally
                {
                    semaphore.Release();
                }
            }).ToArray();

            Task.WaitAll(tasks);

        }

        public async Task DownloadAsync(DownloadableFile file)
        {
            file.status = DownloadableFile.DownloadStatus.Downloading;
            var url = file.fileUrl;
            var videoName = file.fileName;
            downloadIndex++;

            try
            {
                var youtube = YouTube.Default;

                if (url.Contains("www.youtube.com"))
                {
                    var video = await youtube.GetVideoAsync(url);
                    await file.downloadType(file, video, downloadPath);
                    file.status = DownloadableFile.DownloadStatus.Finished;
                }
            }
            catch (Exception e)
            {
                Console.Error.WriteLine($"Download failed ({downloadIndex}) ({videoName}) - {e.GetType()}");
            }

            file.status = DownloadableFile.DownloadStatus.Failed;
        }
    }
}

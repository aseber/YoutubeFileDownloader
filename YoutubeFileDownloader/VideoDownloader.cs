using NReco.VideoConverter;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VideoLibrary;

namespace YoutubeFileDownloader
{
    class VideoDownloader
    {
        private int downloadIndex = 0;

        public class DownloadableFile
        {
            public enum DownloadStatus {

                WAITING,
                DOWNLOADING,
                FINISHED,
                FAILED

            }

            internal string fileName { get; }
            internal string fileUrl { get; }
            public DownloadStatus status { get; internal set; }

            public DownloadableFile(string fileName, string fileUrl)
            {
                this.fileName = fileName;
                this.fileUrl = fileUrl;
                this.status = DownloadStatus.WAITING;
            }
        }

        ConsoleWriter consoleWriter = new ConsoleWriter();

        public void DownloadVideosAsync(IEnumerable<DownloadableFile> files, int concurrentDownloads = 4)
        {
            var semaphore = new SemaphoreSlim(concurrentDownloads);

            var tasks = files.Select(async file =>
            {
                await semaphore.WaitAsync();

                try
                {
                    await DownloadVideoAsync(file);
                }
                finally
                {
                    semaphore.Release();
                }
            }).ToArray();

            Task.WaitAll(tasks);

        }

        public async Task DownloadVideoAsync(DownloadableFile file)
        {
            file.status = DownloadableFile.DownloadStatus.DOWNLOADING;
            var url = file.fileUrl;
            var videoName = file.fileName;
            downloadIndex++;

            try
            {
                var youtube = YouTube.Default;
                var ffMpeg = new FFMpegConverter();

                if (url.Contains("www.youtube.com"))
                {
                    var video = await youtube.GetVideoAsync(url);
                    var fullVideoName = video.FullName;

                    consoleWriter.WriteMessage($"Starting Download ({downloadIndex}) ({videoName})");

                    File.WriteAllBytes("..\\..\\Data\\Results\\" + fullVideoName, await video.GetBytesAsync());
                    ffMpeg.ConvertMedia("..\\..\\Data\\Results\\" + fullVideoName, "..\\..\\Data\\Results\\" + videoName + ".mp3", "mp3");
                    File.Delete("..\\..\\Data\\Results\\" + fullVideoName);

                    file.status = DownloadableFile.DownloadStatus.FINISHED;
                }
            }
            catch (Exception e)
            {
                consoleWriter.WriteError($"Download failed ({downloadIndex}) ({videoName}) - {e.GetType()}");
            }

            file.status = DownloadableFile.DownloadStatus.FAILED;
        }
    }
}

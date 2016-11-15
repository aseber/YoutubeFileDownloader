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
            public string fileName { get; }
            public string fileUrl { get; }

            public DownloadableFile(string fileName, string fileUrl)
            {
                this.fileName = fileName;
                this.fileUrl = fileUrl;
            }
        }

        ConsoleWriter consoleWriter = new ConsoleWriter();

        public void DownloadVideosAsync(IEnumerable<DownloadableFile> files, int concurrentDownloads = 4)
        {
            var semaphore = new SemaphoreSlim(concurrentDownloads);
            var failedDownloads = new List<string>();

            var tasks = files.Select(async file =>
            {
                await semaphore.WaitAsync();

                try
                {
                    var result = await DownloadVideoAsync(file);

                    if (result != string.Empty)
                    {
                        failedDownloads.Add(result);
                    }
                }
                finally
                {
                    semaphore.Release();
                }
            }).ToArray();

            Task.WaitAll(tasks);

            consoleWriter.WriteMessage("Finished all downloads");

            foreach (var failedDownload in failedDownloads)
            {
                consoleWriter.WriteError($"\t{failedDownload} failed to download");
            }

            Console.ReadLine();
        }

        public async Task<string> DownloadVideoAsync(DownloadableFile file)
        {
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
                }
            }
            catch (Exception e)
            {
                consoleWriter.WriteError($"Download failed ({downloadIndex}) ({videoName}) - {e.GetType()}");
                return videoName;
            }

            return string.Empty;
        }
    }
}

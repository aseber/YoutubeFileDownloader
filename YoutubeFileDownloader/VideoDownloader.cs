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
        ConsoleWriter consoleWriter = new ConsoleWriter();

        public void DownloadVideosAsync(IEnumerable<Tuple<string, string, int>> urlTuples, int concurrentDownloads = 4)
        {
            var semaphore = new SemaphoreSlim(concurrentDownloads);
            var failedDownloads = new List<string>();

            var tasks = urlTuples.Select(async urlTuple =>
            {
                await semaphore.WaitAsync();

                try
                {
                    var result = await DownloadVideoAsync(urlTuple);

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

        public async Task<string> DownloadVideoAsync(Tuple<string, string, int> urlTuple)
        {
            var url = urlTuple.Item1;
            var videoName = urlTuple.Item2;
            var videoIndex = urlTuple.Item3;

            try
            {
                var youtube = YouTube.Default;
                var ffMpeg = new FFMpegConverter();

                if (url.Contains("www.youtube.com"))
                {
                    var video = await youtube.GetVideoAsync(url);
                    var fullVideoName = video.FullName;

                    consoleWriter.WriteMessage($"Starting Download ({videoIndex}) ({videoName})");

                    File.WriteAllBytes("..\\..\\Data\\Results\\" + fullVideoName, await video.GetBytesAsync());
                    ffMpeg.ConvertMedia("..\\..\\Data\\Results\\" + fullVideoName, "..\\..\\Data\\Results\\" + videoName + ".mp3", "mp3");
                    File.Delete("..\\..\\Data\\Results\\" + fullVideoName);
                }
            }
            catch (Exception e)
            {
                consoleWriter.WriteError($"Download failed ({videoIndex}) ({videoName}) - {e.GetType()}");
                return videoName;
            }

            return string.Empty;
        }
    }
}

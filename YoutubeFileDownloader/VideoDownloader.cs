using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VideoLibrary;

namespace YoutubeFileDownloaderApi
{
    public class YoutubeDownloader
    {
        private readonly string downloadPath;
        private int downloadIndex = 0;

        public YoutubeDownloader(string downloadPath)
        {
            this.downloadPath = downloadPath;
        }

        public void DownloadAsync(IEnumerable<DownloadableFile> files, int concurrentDownloads = 4)
        {
            var semaphore = new SemaphoreSlim(concurrentDownloads);

            files.Select(async file =>
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
        }

        public async Task DownloadAsync(DownloadableFile file)
        {
            var url = file.fileUrl;
            var videoName = file.videoName;
            downloadIndex++;

            try
            {
                var youtube = YouTube.Default;

                await file.handleDownload(file, downloadPath);
                return;
            }
            catch (Exception e)
            {
                Console.Error.WriteLine($"Download failed ({downloadIndex}) ({videoName}) - {e.GetType()}");
            }

            file.status = DownloadableFile.DownloadStatus.Failed;
        }
    }
}

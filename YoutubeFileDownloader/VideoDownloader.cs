using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using VideoLibrary;

namespace YoutubeFileDownloaderApi
{
    public class YoutubeDownloader
    {

        private BufferBlock<DownloadableFile> buffer = new BufferBlock<DownloadableFile>();
        private readonly string downloadPath;
        private int concurrentDownloads;
        private int downloadIndex = 0;
        private Thread workerThread;

        public YoutubeDownloader(string downloadPath, int concurrentDownloads = 4)
        {
            this.downloadPath = downloadPath;
            this.concurrentDownloads = concurrentDownloads;

            workerThread = new Thread(GetThreadStart());
            workerThread.Start();
        }

        ~YoutubeDownloader()
        {
            workerThread.Abort();
        }

        private ThreadStart GetThreadStart()
        {
            return async () =>
            {
                var semaphore = new SemaphoreSlim(concurrentDownloads);

                try
                {
                    while (true)
                    {
                        var file = await buffer.ReceiveAsync();

                        await semaphore.WaitAsync();

                        try
                        {
                            await DoDownload(file);
                        }
                        finally
                        {
                            semaphore.Release();
                        }
                    }
                }
                catch (ThreadAbortException)
                {
                }
            };
        }

        public void DownloadAsync(IEnumerable<DownloadableFile> files)
        {
            foreach (var file in files)
            {
                buffer.Post(file);
            }
        }

        public void DownloadAsync(DownloadableFile file)
        {
            buffer.Post(file);
        }

        public async Task DoDownload(DownloadableFile file)
        {
            var url = file.fileUrl;
            var videoName = file.videoName;
            downloadIndex++;

            try
            {
                file.status = DownloadableFile.DownloadStatus.Downloading;
                var videoStream = new MemoryStream(await file.videoHandle.GetBytesAsync());
                // var videoStream = await file.videoHandle.StreamAsync(); // I would like to use this method, but StreamAsync currently doesn't work.

                await file.DoSave(file, videoStream, downloadPath);
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

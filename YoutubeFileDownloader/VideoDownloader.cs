using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace YouTubeFileDownloaderApi
{
    public class YoutubeDownloader
    {

        private readonly BufferBlock<DownloadableFile> buffer = new BufferBlock<DownloadableFile>();
        private readonly int concurrentDownloads;
        private readonly SemaphoreSlim semaphore;
        private readonly Thread[] workerThreads;
        private int downloadIndex = 0;
        public string downloadPath { get; set; }


        public YoutubeDownloader(string downloadPath, int concurrentDownloads = 4)
        {
            this.downloadPath = downloadPath;
            this.concurrentDownloads = concurrentDownloads;
            semaphore = new SemaphoreSlim(concurrentDownloads);

            workerThreads = new Thread[concurrentDownloads];

            for (int i = 0; i < concurrentDownloads; i++)
            {
                var workerThread = new Thread(GetThreadStart());
                workerThread.Name = "VideoDownloader Worker Thread (" + base.ToString() + ") #" + i;
                workerThread.Start();
                workerThreads[i] = workerThread;
            }

        }

        ~YoutubeDownloader()
        {
            for (int i = 0; i < concurrentDownloads; i++)
            {
                workerThreads[i].Abort();
            }
        }

        private ThreadStart GetThreadStart()
        {
            return async () =>
            {
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
                DownloadAsync(file);
            }
        }

        public void DownloadAsync(DownloadableFile file)
        {
            buffer.Post(file);
        }

        public async Task DoDownload(DownloadableFile file)
        {
            downloadIndex++;

            try
            {
                await file.DoAsyncSave(file, downloadPath);
                return;
            }
            catch (Exception e)
            {
                Console.Error.WriteLine($"Download failed ({downloadIndex}) ({file.GetVideoName()}) - {e.GetType()}");
            }

            file.status = DownloadableFile.DownloadStatus.Failed;
        }
    }
}

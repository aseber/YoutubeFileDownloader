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
        private readonly int concurrentDownloads;
        private readonly SemaphoreSlim semaphore;
        private int downloadIndex = 0;
        private Thread[] workerThreads;

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

                //using (var videoStream = await file.videoHandle.StreamAsync()) // I would like to use this method, but StreamAsync currently doesn't work.
                using (var videoStream = new MemoryStream(await file.videoHandle.GetBytesAsync()))
                {
                    await file.DoSave(file, videoStream, downloadPath);
                }
                
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

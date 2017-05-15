using NReco.VideoConverter;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static YouTubeFileDownloaderApi.DownloadableFile;

namespace YouTubeFileDownloaderApi
{
    public class AsyncSaveHandlerFactory
    {
        private static AsyncSaveHandlerFactory singleton = new AsyncSaveHandlerFactory();

        private AsyncSaveHandlerFactory() { }

        public static AsyncSaveHandlerFactory GetInstance()
        {
            return singleton;
        }

        public AsyncSaveHandler GetAudioSaveHandler()
        {
            return AudioHandler;
        }

        public AsyncSaveHandler GetVideoSaveHandler()
        {
            return VideoHandler;
        }

        public delegate Task AsyncSaveHandler(DownloadableFile file, string downloadPath);

        private static readonly AsyncSaveHandler AudioHandler = async (DownloadableFile file, string downloadPath) =>
        {
            var streamInfo = file.videoHandle.AudioStreams.OrderBy(s => s.Bitrate).Last();

            var fileName = file.GetVideoName();
            string fileExtension = streamInfo.Container.ToString();

            string intermediateFileName = downloadPath + fileName + "." + fileExtension;
            string finalFileName = downloadPath + fileName + ".mp3";

            file.status = DownloadStatus.Downloading;

            using (var inputStream = await file.GetStreamAsync(streamInfo))
            {
                using (var fileStream = new FileStream(intermediateFileName, FileMode.Create, FileAccess.Write))
                {
                    await inputStream.CopyToAsync(fileStream);
                }
            }

            file.status = DownloadStatus.Converting;
            await Task.Factory.StartNew(() => { new FFMpegConverter().ConvertMedia(intermediateFileName, finalFileName, "mp3"); });
            File.Delete(intermediateFileName);

            file.status = DownloadStatus.Completed;
        };

        private static readonly AsyncSaveHandler VideoHandler = async (DownloadableFile file, string downloadPath) =>
        {
            var streamInfo = file.videoHandle.MixedStreams.OrderBy(s => s.VideoQuality).Last();

            var fileName = file.GetVideoName();
            string fileExtension = streamInfo.Container.ToString();

            string finalFileName = downloadPath + fileName + "." + fileExtension;

            file.status = DownloadStatus.Downloading;

            using (var inputStream = await file.GetStreamAsync(streamInfo))
            {
                using (var fileStream = new FileStream(finalFileName, FileMode.Create, FileAccess.Write))
                {
                    await inputStream.CopyToAsync(fileStream);
                }
            }

            file.status = DownloadStatus.Completed;
        };
    }
}

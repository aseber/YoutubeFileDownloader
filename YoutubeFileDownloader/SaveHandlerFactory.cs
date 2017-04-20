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
    public class SaveHandlerFactory
    {
        private static SaveHandlerFactory singleton = new SaveHandlerFactory();

        private SaveHandlerFactory() { }

        public static SaveHandlerFactory GetInstance()
        {
            return singleton;
        }

        public SaveHandler GetAudioSaveHandler()
        {
            return AudioHandler;
        }

        public SaveHandler GetVideoSaveHandler()
        {
            return VideoHandler;
        }

        public delegate Task SaveHandler(DownloadableFile file, Stream videoStream, string downloadPath);

        private static readonly SaveHandler AudioHandler = async (DownloadableFile file, Stream videoStream, string downloadPath) =>
        {
            var videoName = file.videoName;

            string videoFileName = downloadPath + videoName + ".data";
            string audioFileName = downloadPath + videoName + ".mp3";

            file.status = DownloadStatus.Downloading;

            using (var fileStream = new FileStream(videoFileName, FileMode.Create, FileAccess.Write))
            {
                await videoStream.CopyToAsync(fileStream);
            }

            file.status = DownloadStatus.Converting;
            await Task.Factory.StartNew(() => { new FFMpegConverter().ConvertMedia(videoFileName, audioFileName, "mp3"); });
            File.Delete(videoFileName);

            file.status = DownloadStatus.Completed;
        };

        private static readonly SaveHandler VideoHandler = async (DownloadableFile file, Stream videoStream, string downloadPath) =>
        {
            var videoName = file.videoName;
            var videoExtension = file.videoHandle.FileExtension;

            string videoFileName = downloadPath + videoName + videoExtension;

            file.status = DownloadStatus.Downloading;

            using (var fileStream = new FileStream(videoFileName, FileMode.Create, FileAccess.Write))
            {
                await videoStream.CopyToAsync(fileStream);
            }

            file.status = DownloadStatus.Completed;
        };
    }
}

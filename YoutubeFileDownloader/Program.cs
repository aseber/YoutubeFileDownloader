using System.Collections.Generic;

namespace YoutubeFileDownloaderApi
{
    class Program
    {
        static void Main()
        {
            var videoDownloader = new YoutubeDownloader("..\\..\\Data\\Results\\");
            videoDownloader.DownloadAsync(UrlList());
        }

        static IEnumerable<YoutubeDownloader.DownloadableFile> UrlList()
        {
            yield return new YoutubeDownloader.DownloadableFile("Coldplay - Everglow", "https://www.youtube.com/watch?v=P8jOQUsTU9o", YoutubeDownloader.Video);
        }
    }
}
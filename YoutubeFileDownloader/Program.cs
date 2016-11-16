using System;
using System.Collections.Generic;

namespace YoutubeFileDownloaderApi
{
    class Program
    {
        static void Main()
        {
            var videoDownloader = new YoutubeDownloader("..\\..\\Data\\Results\\");
            videoDownloader.DownloadAsync(UrlList());
            Console.ReadLine();
        }

        static IEnumerable<DownloadableFile> UrlList()
        {
            yield return new DownloadableFile("https://www.youtube.com/watch?v=P8jOQUsTU9o", DownloadableFile.Audio);
        }
    }
}
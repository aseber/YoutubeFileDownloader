using HtmlAgilityPack;
using NReco.VideoConverter;
using System.Threading.Tasks;
using System;
using System.IO;
using System.Linq;
using VideoLibrary;
using System.Collections.Generic;
using System.Threading;

namespace YoutubeFileDownloader
{
    class Program
    {
        static void Main()
        {
            var videoDownloader = new VideoDownloader();
            videoDownloader.DownloadVideosAsync(UrlList());
        }

        static IEnumerable<VideoDownloader.DownloadableFile> UrlList()
        {
            yield return new VideoDownloader.DownloadableFile("", "");
        }
    }
}
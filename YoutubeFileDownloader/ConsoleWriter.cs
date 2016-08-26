using System;

namespace YoutubeFileDownloader
{
    public class ConsoleWriter
    {
        private static object _MessageLock = new object();

        public void WriteMessage(string message)
        {
            lock (_MessageLock)
            {
                Console.WriteLine(message);
            }
        }

        public void WriteError(string message)
        {
            lock (_MessageLock)
            {
                Console.BackgroundColor = ConsoleColor.Red;
                Console.WriteLine(message);
                Console.ResetColor();
            }
        }
    }
}

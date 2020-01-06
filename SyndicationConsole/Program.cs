using Syndication.Rss;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace SyndicationConsole
{
    /// <summary>
    /// This console application is just for debugging and development purposes. Not part of the demo application.
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                MainAsync(args).Wait();
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
            }
        }

        static async Task MainAsync(string[] args)
        {
            var items = await RssItem.LoadAsync("https://mikaberglund.com/feed/", 3, new HttpClient());
        }
    }
}

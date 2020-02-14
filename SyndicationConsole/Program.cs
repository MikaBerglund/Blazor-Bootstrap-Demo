using Syndication;
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
            var rssUrl = "https://mikaberglund.com/feed/";
            var atomUrl = "https://tulli.fi/tietoa-tullista/uutishuone/-/asset_publisher/vSckabkfdtUg/rss";

            //var rssFeed = await Feed.LoadAsync(rssUrl);
            var atomFeed = await Feed.LoadAsync(atomUrl);

        }
    }
}

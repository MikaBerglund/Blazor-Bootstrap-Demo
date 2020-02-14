using Syndication.Atom;
using Syndication.Rss;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Syndication
{
    public abstract class Feed
    {

        protected Feed()
        {
            this.Items = new List<FeedItem>();
        }


        public string Description { get; protected set; }

        public string Title { get; protected set; }

        public string Url { get; protected set; }


        public ICollection<FeedItem> Items { get; private set; }


        public static async Task<Feed> LoadAsync(string url)
        {
            Feed feed = null;
            var http = new HttpClient();
            var response = await http.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var xml = new XmlDocument();
                xml.LoadXml(await response.Content.ReadAsStringAsync());

                if(xml.DocumentElement.NamespaceURI == AtomFeed.XmlNamespace)
                {
                    feed = new AtomFeed(xml);
                }
                else
                {
                    feed = new RssFeed(xml);
                }
            }

            return feed;
        }


        public override string ToString()
        {
            return this.Title ?? base.ToString();
        }
    }
}

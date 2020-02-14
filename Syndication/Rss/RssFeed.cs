using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Syndication.Rss
{
    public class RssFeed : Feed
    {
        internal RssFeed(XmlDocument feedDocument)
        {
            this.Title = feedDocument.DocumentElement.SelectSingleNode("channel/title")?.InnerText;
            this.Url = feedDocument.DocumentElement.SelectSingleNode("channel/link")?.InnerText;
            this.Description = feedDocument.DocumentElement.SelectSingleNode("channel/description")?.InnerText;

            foreach(XmlNode node in feedDocument.DocumentElement.SelectNodes("channel/item"))
            {
                var item = new FeedItem()
                {
                    Title = node.SelectSingleNode("title")?.InnerText,
                    Url = node.SelectSingleNode("link")?.InnerText
                };

                this.Items.Add(item);
            }
        }

    }
}

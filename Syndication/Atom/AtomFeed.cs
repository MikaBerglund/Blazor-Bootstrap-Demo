using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Syndication.Atom
{
    public class AtomFeed : Feed
    {
        internal AtomFeed(XmlDocument feedDocument)
        {
            var nsMan = new XmlNamespaceManager(feedDocument.NameTable);
            nsMan.AddNamespace("atom", AtomFeed.XmlNamespace);

            this.Title = feedDocument.DocumentElement.SelectSingleNode("atom:title", nsMan)?.InnerText;
            this.Url = feedDocument.DocumentElement.SelectSingleNode("atom:link[@rel = 'alternate']/@href", nsMan)?.InnerText;

            foreach (XmlNode node in feedDocument.DocumentElement.SelectNodes("atom:entry", nsMan))
            {
                var item = new FeedItem
                {
                    Title = node.SelectSingleNode("atom:title", nsMan)?.InnerText,
                    Url = node.SelectSingleNode("atom:link[@rel = 'alternate']/@href", nsMan)?.InnerText
                };

                this.Items.Add(item);
            }
        }

        public const string XmlNamespace = "http://www.w3.org/2005/Atom";

    }
}

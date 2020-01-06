using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Xml;

namespace Syndication.Rss
{
    public class RssItem
    {
        public RssItem()
        {
            this.Tags = new List<string>();
        }


        public string Category { get; set; }

        public string Description { get; set; }

        public string ImageUrl { get; set; }

        public DateTime PublishDate { get; set; }

        public ICollection<string> Tags { get; set; }

        public string Title { get; set; }

        public string Url { get; set; }



        public static async Task<IEnumerable<RssItem>> LoadAsync(string feedUrl, int? maxEntries, HttpClient httpClient)
        {
            var items = new List<RssItem>();

            var request = new HttpRequestMessage(HttpMethod.Get, feedUrl);
            var response = await httpClient.SendAsync(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var xml = await response.Content.ReadAsStringAsync();

                var xDoc = new XmlDocument();
                xDoc.LoadXml(xml);

                int itemCount = 0;
                foreach (XmlNode item in xDoc.DocumentElement.SelectNodes("channel/item"))
                {
                    var title = item.SelectSingleNode("title")?.InnerText;
                    var link = item.SelectSingleNode("link")?.InnerText;
                    var pubDate = item.SelectSingleNode("pubDate")?.InnerText;
                    DateTime.TryParse(pubDate, out DateTime dt);

                    var meta = await LoadMetaAsync(link, httpClient);

                    var rssItem = new RssItem { Title = title, Url = link, PublishDate = dt };
                    rssItem.ImageUrl = meta.FirstOrDefault(x => x.Key == "og:image").Value;
                    rssItem.Description = meta.FirstOrDefault(x => x.Key == "description").Value;
                    rssItem.Category = meta.FirstOrDefault(x => x.Key == "article:section").Value;
                    rssItem.Tags = new List<string>(from x in meta where x.Key == "article:tag" select x.Value);
                    items.Add(rssItem);

                    itemCount++;
                    if (maxEntries.HasValue && itemCount >= maxEntries)
                    {
                        break;
                    }
                }
            }

            return items;
        }

        private static async Task<IEnumerable<KeyValuePair<string, string>>> LoadMetaAsync(string url, HttpClient httpClient)
        {
            var meta = new List<KeyValuePair<string, string>>();

            var request = new HttpRequestMessage(HttpMethod.Get, url);
            var response = await httpClient.SendAsync(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var html = await response.Content.ReadAsStringAsync();
                foreach (Match match in Regex.Matches(html, @"(<meta)[^>]+\/>"))
                {
                    XmlDocument xDoc = new XmlDocument();
                    xDoc.LoadXml(HttpUtility.HtmlDecode(match.Value));
                    var name = xDoc.DocumentElement.GetAttribute("name");
                    if (string.IsNullOrEmpty(name)) name = xDoc.DocumentElement.GetAttribute("property");

                    meta.Add(new KeyValuePair<string, string>(name, xDoc.DocumentElement.GetAttribute("content")));
                }
            }

            return meta;
        }

    }
}

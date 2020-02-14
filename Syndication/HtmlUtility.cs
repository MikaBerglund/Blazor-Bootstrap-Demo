using AngleSharp.Html.Parser;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Xml;

namespace Syndication
{
    public static class HtmlUtility
    {

        private static HttpClient Client = new HttpClient();
        public static async Task<string> DownloadHtmlAsync(string url)
        {
            var response = await Client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }

            return null;
        }

        public static async Task<WebPageMetadata> ParseMetadataAsync(Uri url)
        {
            var html = await DownloadHtmlAsync(url.ToString());
            var meta = await ParseMetadataAsync(html);
            meta.Url = url.ToString();

            if (!string.IsNullOrEmpty(meta.ImageUrl))
            {
                var uri = new Uri(meta.ImageUrl, UriKind.RelativeOrAbsolute);
                if(!uri.IsAbsoluteUri)
                {
                    var imageUri = new Uri(url, meta.ImageUrl);
                    meta.ImageUrl = imageUri.ToString();
                }
            }
            return meta;
        }

        private static async Task<WebPageMetadata> ParseMetadataAsync(string html)
        {
            var meta = new WebPageMetadata();

            var parser = new HtmlParser();
            var document = await parser.ParseDocumentAsync(html);

            try
            {
                meta.Title =
                    document.QuerySelector("meta[property = 'og:title']")?.Attributes?.GetNamedItem("content")?.Value
                    ?? document.QuerySelector("title").InnerHtml
                    ;

                meta.Description =
                    document.QuerySelector("meta[property = 'og:description']")?.Attributes?.GetNamedItem("content")?.Value
                    ?? document.QuerySelector("meta[name='description']")?.Attributes?.GetNamedItem("content")?.Value
                    ;

                meta.ImageUrl = document.QuerySelector("meta[property = 'og:image']")?.Attributes?.GetNamedItem("content")?.Value;

                meta.Category = document.QuerySelector("meta[property = 'article:section']")?.Attributes?.GetNamedItem("content")?.Value;

                var tagMetas = document.QuerySelectorAll("meta[property = 'article:tag']");
                var tagList = new List<string>();
                foreach(var tag in tagMetas)
                {
                    var t = tag.GetAttribute("content");
                    if (!string.IsNullOrEmpty(t))
                    {
                        tagList.Add(t);
                    }
                }
                meta.Tags = tagList;

                var publishedTime = 
                    document.QuerySelector("meta[property = 'article:published_time']")?.Attributes?.GetNamedItem("content")?.Value
                    ?? document.QuerySelector("meta[property = 'og:article:published_time']")?.Attributes?.GetNamedItem("content")?.Value
                    ;

                if(DateTimeOffset.TryParse(publishedTime, out DateTimeOffset dt))
                {
                    meta.Published = dt;
                }
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                throw;
            }

            return meta;
        }
    }
}

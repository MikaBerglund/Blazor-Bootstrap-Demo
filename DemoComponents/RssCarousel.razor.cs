using Microsoft.AspNetCore.Components;
using Syndication;
using Syndication.Rss;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DemoComponents
{
    partial class RssCarousel
    {

        public RssCarousel()
        {
            this.ResetItems();
        }


        [Parameter]
        public ICollection<WebPageMetadata> Items { get; set; }

        private string _FeedUrl;
        [Parameter]
        public string FeedUrl
        {
            get { return _FeedUrl; }
            set
            {
                _FeedUrl = value;
                this.ResetItems();
            }
        }

        [Parameter]
        public int? MaxItems { get; set; }



        private string CurrentUrl;
        protected override void OnParametersSet()
        {
            if(this.FeedUrl != this.CurrentUrl)
            {
                this.CurrentUrl = this.FeedUrl;
                this.LoadItemsAsync(this.CurrentUrl);
            }

            base.OnParametersSet();
        }


        private async Task LoadItemsAsync(string url)
        {
            this.ResetItems();
            var feed = await Feed.LoadAsync(url);
            foreach(var item in feed.Items)
            {
                if (this.CurrentUrl == this.FeedUrl)
                {
                    var meta = await HtmlUtility.ParseMetadataAsync(new Uri(item.Url));
                    this.Items.Add(meta);
                    this.StateHasChanged();
                }
            }
        }

        private void ResetItems()
        {
            this.Items = new List<WebPageMetadata>();
        }

    }
}

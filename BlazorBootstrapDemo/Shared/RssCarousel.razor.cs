using Microsoft.AspNetCore.Components;
using Syndication;
using Syndication.Rss;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorBootstrapDemo.Shared
{
    partial class RssCarousel
    {

        public RssCarousel()
        {
            this.ResetItems();
        }


        [Parameter]
        public ICollection<RssItem> Items { get; set; }

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

        protected async override Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();

            var currentUrl = this.FeedUrl;
            var feed = await Feed.LoadAsync(currentUrl);

        }


        private void ResetItems()
        {
            this.Items = new List<RssItem>();
        }

    }
}

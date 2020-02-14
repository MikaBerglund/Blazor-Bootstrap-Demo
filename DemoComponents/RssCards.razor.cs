using Microsoft.AspNetCore.Components;
using Syndication.Rss;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace DemoComponents
{
    partial class RssCards
    {

        private IEnumerable<RssItem> Items { get; set; }

        private string _FeedUrl;
        [Parameter]
        public string FeedUrl
        {
            get { return _FeedUrl; }
            set
            {
                _FeedUrl = value;
                this.Items = null;
            }
        }

        [Inject]
        protected Blazored.SessionStorage.ISessionStorageService Storage { get; set; }

        private const string ItemsExpireProperty = "RssItemsExpire";
        private const string ItemsProperty = "RssItems";

        protected override async Task OnParametersSetAsync()
        {

            if (null == this.Items && !string.IsNullOrEmpty(this.FeedUrl))
            {
                var expires = await this.Storage.GetItemAsync<DateTime?>(ItemsExpireProperty);
                var items = await this.Storage.GetItemAsync<IEnumerable<RssItem>>(ItemsProperty);

                if (expires.GetValueOrDefault() < DateTime.UtcNow || null == items)
                {
                    await this.Storage.SetItemAsync(ItemsExpireProperty, DateTime.UtcNow.AddHours(1));
                    items = await RssItem.LoadAsync(this.FeedUrl, 3, new HttpClient());
                    await this.Storage.SetItemAsync(ItemsProperty, items);
                }

                this.Items = items;
            }

            await base.OnParametersSetAsync();
        }
    }
}

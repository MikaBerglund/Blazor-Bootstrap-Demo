using Blazorade.Bootstrap.Components;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace BlazorBootstrapDemo.Shared
{
    public class WebPageCarouselItem : CarouselItem
    {

        /// <summary>
        /// The absolute URL of the webpage to show in the carousel item.
        /// </summary>
        [Parameter]
        public string Url { get; set; }

        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();

            this.CaptionHeading = null;
            this.CaptionBody = null;

            var html = await this.DownloadHtmlAsync(this.Url);
            if(!string.IsNullOrEmpty(html))
            {

            }
        }


        private async Task<string> DownloadHtmlAsync(string url)
        {


            return null;
        }
    }
}

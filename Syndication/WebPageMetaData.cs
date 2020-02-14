using System;
using System.Collections.Generic;
using System.Text;

namespace Syndication
{
    public class WebPageMetadata
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public string Url { get; set; }

        public string ImageUrl { get; set; }

        public DateTimeOffset? Published { get; set; }

        public string Category { get; set; }

        public IReadOnlyCollection<string> Tags { get; set; }

        public override string ToString()
        {
            return this.Title ?? base.ToString();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Syndication
{
    public class FeedItem
    {

        public string Title { get; set; }

        public string Url { get; set; }


        public override string ToString()
        {
            return this.Title ?? base.ToString();
        }
    }
}

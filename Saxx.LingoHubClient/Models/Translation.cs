using System;
using System.Collections.Generic;
using System.Linq;

namespace Saxx.LingoHubClient.Models
{
    public class Translation
    {
        public string Title { get; set; }

        public IEnumerable<Link> Links { get; set; }

        public Uri Href
        {
            get
            {
                return new Uri(Links.First(x => x.Rel == "self").Href);
            }
        }
    }
}
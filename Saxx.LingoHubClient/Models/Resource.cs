using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Saxx.LingoHubClient.Models
{
    public class Resource
    {
        public string Name { get; set; }

        [JsonProperty("project_locale")]
        public string Locale { get; set; }

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

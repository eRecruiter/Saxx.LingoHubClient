using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Saxx.LingoHubClient.Models
{
    public class ProjectDetails
    {
        public string Title { get; set; }

        [JsonProperty("owner_email")]
        public string OwnerEmail { get; set; }

        public string Description { get; set; }

        public bool OpenSource { get; set; }

        [JsonProperty("project_locales")]
        public IEnumerable<string> ProjectLocales { get; set; }

        [JsonProperty("created_at")]
        public DateTime DateOfCreation { get; set; }

        [JsonProperty("updated_at")]
        public DateTime DateOfUpdate { get; set; }

        public IEnumerable<Link> Links { get; set; }

        public Uri HrefResources
        {
            get
            {
                return new Uri(Links.First(x => x.Rel == "resources").Href);
            }
        }

        public Uri HrefTranslations
        {
            get
            {
                return new Uri(Links.First(x => x.Rel == "translations").Href);
            }
        }
    }
}

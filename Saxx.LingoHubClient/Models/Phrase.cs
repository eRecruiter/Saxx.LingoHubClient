using Newtonsoft.Json;

namespace Saxx.LingoHubClient.Models
{
    public class Phrase
    {
        public string Content { get; set; }

        [JsonProperty("iso_locale")]
        public string Locale { get; set; }
    }
}
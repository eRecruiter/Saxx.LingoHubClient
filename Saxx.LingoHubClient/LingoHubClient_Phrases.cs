using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Saxx.LingoHubClient.Exceptions;
using Saxx.LingoHubClient.Models;

namespace Saxx.LingoHubClient
{
    public partial class LingoHubClient
    {
        public async Task<IEnumerable<Phrase>> GetPhrases(Translation translation)
        {
            var json = JsonConvert.DeserializeObject<JObject>(await GetStringAsync(translation.Href + ".json"));
            return from x in json.Value<JArray>("phrases")
                select x.ToObject<Phrase>();
        }
    }
}
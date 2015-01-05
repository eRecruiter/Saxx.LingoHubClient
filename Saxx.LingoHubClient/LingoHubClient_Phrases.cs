using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Saxx.LingoHubClient.Exceptions;
using Saxx.LingoHubClient.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Saxx.LingoHubClient
{
    public partial class LingoHubClient
    {
        public async Task<IEnumerable<Phrase>> GetPhrases(string projectTitle, string translationTitle)
        {
            var translations = await GetTranslations(projectTitle);
            var translation = translations.FirstOrDefault(x => x.Title.Equals(translationTitle, StringComparison.InvariantCultureIgnoreCase));

            if (translation == null)
                throw new TranslationDoesNotExistException(projectTitle, translationTitle);

            return await GetPhrases(translation);
        }


        public async Task<IEnumerable<Phrase>> GetPhrases(Translation translation)
        {
            var json = JsonConvert.DeserializeObject<JObject>(await GetStringAsync(translation.Href + ".json"));
            return from x in json.Value<JArray>("phrases")
                   select x.ToObject<Phrase>();
        }
    }
}

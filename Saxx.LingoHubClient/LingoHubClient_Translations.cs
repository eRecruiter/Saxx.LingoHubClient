using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Saxx.LingoHubClient.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Saxx.LingoHubClient
{
    public partial class LingoHubClient
    {
        public async Task<IEnumerable<Translation>> GetTranslations(string projectTitle)
        {
            return await GetTranslations(await GetProjectDetails(projectTitle));
        }

        public async Task<IEnumerable<Translation>> GetTranslations(ProjectDetails project)
        {
            var json = JsonConvert.DeserializeObject<JObject>(await GetStringAsync(project.HrefTranslations + ".json?page_size=" + int.MaxValue));
            return from x in json.Value<JArray>("members")
                   select x.ToObject<Translation>();
        }
    }
}

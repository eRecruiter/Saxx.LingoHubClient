using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Saxx.LingoHubClient.Models;

namespace Saxx.LingoHubClient
{
    public partial class LingoHubClient
    {
        public async Task<IEnumerable<Resource>> GetResources(string projectTitle)
        {
            return await GetResources(await GetProjectDetails(projectTitle));
        }


        public async Task<IEnumerable<Resource>> GetResources(ProjectDetails project)
        {
            var json = JsonConvert.DeserializeObject<JObject>(await GetStringAsync(project.HrefResources + ".json"));
            return from x in json.Value<JArray>("members")
                   select x.ToObject<Resource>();
        }


        public async Task<byte[]> DownloadResource(Resource resource)
        {
            return await GetByteArrayAsync(resource.Href);
        }


        public async Task UploadResource(string projectTitle, string filePath, string niceFileName = null, string forceLocale = null)
        {
            await UploadResource(await GetProjectDetails(projectTitle), filePath, niceFileName, forceLocale);
        }


        public async Task UploadResource(ProjectDetails project, string filePath, string niceFileName = null, string forceLocale = null)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException("File not found.", filePath);

            if (string.IsNullOrEmpty(niceFileName))
                niceFileName = Path.GetFileName(filePath);

            var fileContent = new ByteArrayContent(File.ReadAllBytes(filePath));
            fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("text/xml");

            var postContent = new MultipartFormDataContent
            {
                {
                    fileContent, "file", niceFileName
                }
            };

            var url = project.HrefResources.ToString();
            if (!string.IsNullOrEmpty(forceLocale))
                url += "?iso2_code=" + forceLocale;

            var postResult = await PostAsync(url, postContent);
            postResult.EnsureSuccessStatusCode();
        }
    }
}
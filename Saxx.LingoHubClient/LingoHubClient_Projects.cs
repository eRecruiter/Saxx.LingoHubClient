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
        public async Task<IEnumerable<Project>> GetProjects()
        {
            var json = JsonConvert.DeserializeObject<JObject>(await GetStringAsync("projects"));
            return from x in json.Value<JArray>("members")
                select x.ToObject<Project>();
        }


        public async Task<Project> GetProject(string projectTitle)
        {
            if (string.IsNullOrEmpty(projectTitle))
                throw new ArgumentOutOfRangeException("projectTitle");

            var projects = await GetProjects();
            var project = projects.FirstOrDefault(x => x.Title.Equals(projectTitle, StringComparison.InvariantCultureIgnoreCase));

            if (project == null)
                throw new ProjectDoesNotExistException(projectTitle);

            return project;
        }


        public async Task<ProjectDetails> GetProjectDetails(string projectTitle)
        {
            return await GetProjectDetails(await GetProject(projectTitle));
        }


        public async Task<ProjectDetails> GetProjectDetails(Project project)
        {
            return JsonConvert.DeserializeObject<ProjectDetails>(await GetStringAsync(project.Href));
        }
    }
}
using System;
using System.IO;
using System.Linq;
using Saxx.LingoHubSyncer.Resx;

namespace Saxx.LingoHubSyncer
{
    public class DownloadService : IDisposable
    {
        #region Constructor

        private readonly LingoHubClient.LingoHubClient _client;
        private readonly ExpandResxService _expandService;


        public DownloadService()
        {
            _client = new LingoHubClient.LingoHubClient(Program.Configuration.Username, Program.Configuration.Password);
            _expandService = new ExpandResxService();
        }


        public void Dispose()
        {
            _client.Dispose();
        }

        #endregion

        public void Run()
        {
            var directoryPath = Program.Configuration.Path;
            if (File.Exists(directoryPath))
                throw new Exception("There is already a file '" + directoryPath + "'.");
            if (!Directory.Exists(directoryPath))
                Directory.CreateDirectory(directoryPath);

            var resources = _client.GetResources(Program.Configuration.Project).Result;
            var resource = resources.FirstOrDefault(x => x.Locale == Program.Configuration.Locale && x.Href.AbsoluteUri.Contains("resources/resources"));
            if (resource == null)
                throw new Exception("There is no resource file for locale '" + Program.Configuration.Locale + "'.");

            var tempCombinedResxPath = Path.GetTempFileName();
            File.WriteAllBytes(tempCombinedResxPath, _client.DownloadResource(resource).Result);

            _expandService.ExpandSingleResourceFileToDirectory(directoryPath, tempCombinedResxPath);

            File.Delete(tempCombinedResxPath);
        }
    }
}
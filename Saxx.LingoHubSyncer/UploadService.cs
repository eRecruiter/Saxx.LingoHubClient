using System;
using System.IO;
using Saxx.LingoHubSyncer.Resx;

namespace Saxx.LingoHubSyncer
{
    public class UploadService : IDisposable
    {
        #region Constructor

        private readonly LingoHubClient.LingoHubClient _client;
        private readonly CombineResxService _resxService;


        public UploadService()
        {
            _client = new LingoHubClient.LingoHubClient(Program.Configuration.Username, Program.Configuration.Password);
            _resxService = new CombineResxService();
        }


        public void Dispose()
        {
            _client.Dispose();
        }

        #endregion

        public void Run()
        {
            var path = Program.Configuration.Path;

            var tempResxPath = _resxService.CombineDirectoryToSingleResourceFile(path);

            // always use the same (generic) name, so that LingoHub correctly identifies changed/deleted translations
            var niceFileName = "resources." + Program.Configuration.Locale + ".resx";

            // ReSharper disable once RedundantArgumentName
            _client.UploadResource(Program.Configuration.Project, tempResxPath, niceFileName: niceFileName).Wait();
            File.Delete(tempResxPath);
        }
    }
}
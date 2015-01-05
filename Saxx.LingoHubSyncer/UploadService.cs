using System;
using System.IO;

namespace Saxx.LingoHubSyncer
{
    public class UploadService : IDisposable
    {
        #region Constructor

        private LingoHubClient.LingoHubClient _client;
        private TempResourceService _resxService;


        public UploadService()
        {
            _client = new LingoHubClient.LingoHubClient(Program.Configuration.Username, Program.Configuration.Password);
            _resxService = new TempResourceService();
        }


        public void Dispose()
        {
            _client.Dispose();
        }

        #endregion


        public void Run()
        {
            var path = Program.Configuration.Path;

            var tempResxPath = _resxService.MakeSingleResource(path);
            _client.UploadResource(Program.Configuration.Project, tempResxPath).Wait();
            File.Delete(tempResxPath);
        }
    }
}

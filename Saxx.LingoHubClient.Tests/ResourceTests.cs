using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Saxx.LingoHubClient.Tests
{
    public class ResourceTests : TestBase
    {
        [Test]
        public async Task Upload_Resx()
        {
            var resourcePath = Path.Combine(GetWorkingDirectory(), "Resources", "Some_Resources.de.resx");
            await Client.UploadResource(Configuration.ProjectTitle, resourcePath);
        }


        [Test]
        public async Task Upload_Resx_And_Force_Locale()
        {
            var resourcePath = Path.Combine(GetWorkingDirectory(), "Resources", "Some_Resources.resx");
            await Client.UploadResource(Configuration.ProjectTitle, resourcePath, "en");
        }


        [Test]
        public async Task Download_Resx()
        {
            var resources = (await Client.GetResources(Configuration.ProjectTitle)).ToList();

            var tempPath = Path.GetTempFileName();
            File.WriteAllBytes(tempPath, await Client.DownloadResource(resources.First()));

            Assert.Greater(new FileInfo(tempPath).Length, 0);
            File.Delete(tempPath);
        }


        [Test]
        public async Task Load_Resources()
        {
            var resources = (await Client.GetResources(Configuration.ProjectTitle)).ToList();

            Assert.AreEqual(3, resources.Count());
            Assert.IsTrue(resources.Any(x => x.Name == "Some_Resources.en.resx"));
            Assert.IsTrue(resources.Any(x => x.Name == "Some_Resources.de.resx"));
            Assert.IsTrue(resources.Any(x => x.Name == "Some_Resources.es.resx"));
        }


        private string GetWorkingDirectory()
        {
            // we need this fancy way to make it work in NUnit, because the DLL is executed from some temp path
            var codeBase = GetType().Assembly.EscapedCodeBase;
            var uri = new UriBuilder(codeBase);
            var path = Uri.UnescapeDataString(uri.Path);
            return Path.GetDirectoryName(path);
        }
    }
}
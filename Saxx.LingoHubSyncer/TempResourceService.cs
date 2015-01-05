using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;

namespace Saxx.LingoHubSyncer
{
    public class TempResourceService
    {
        #region Constructor

        private readonly ResxFileNameUtilities _resxService;


        public TempResourceService()
        {
            _resxService = new ResxFileNameUtilities();
        }

        #endregion

        public string MakeSingleResource(string path)
        {
            var resources = new Dictionary<string, string>();

            if (Directory.Exists(path))
            {
                AddResourcesFromDirectoryRecursive(path, path, resources);
            }
            else
            {
                AddResourcesFromFile(path, path, resources);
            }

            return WriteToTemporaryResxFile(resources);
        }


        private void AddResourcesFromDirectoryRecursive(string directoryPath, string basePath, IDictionary<string, string> resources)
        {
            foreach (var filePath in Directory.GetFiles(directoryPath, "*.resx"))
            {
                var fileLocale = _resxService.GetLocale(filePath);
                if ((fileLocale != null && Program.Configuration.Locale.Equals(fileLocale, StringComparison.InvariantCultureIgnoreCase)) ||
                    (fileLocale == null && Program.Configuration.Locale.Equals(Program.Configuration.DefaultLocale, StringComparison.InvariantCultureIgnoreCase)))
                {
                    AddResourcesFromFile(filePath, basePath, resources);
                }
            }

            foreach (var directory in Directory.GetDirectories(directoryPath))
                AddResourcesFromDirectoryRecursive(directory, basePath, resources);
        }


        private void AddResourcesFromFile(string filePath, string basePath, IDictionary<string, string> resources)
        {
            var xml = XDocument.Load(filePath);
            if (xml.Root == null)
                return;

            foreach (var element in xml.Root.Elements("data"))
            {
                var key = _resxService.EncodePathInResourceKey(filePath, basePath, element.Attribute("name").Value);
                var value = element.Element("value");
                if (value == null)
                    continue;
                resources[key] = value.Value;
            }
        }


        // ReSharper disable once ParameterTypeCanBeEnumerable.Local
        private string WriteToTemporaryResxFile(IDictionary<string, string> resources)
        {
            var xml = XDocument.Load(Path.Combine(GetWorkingDirectory(), "Empty_Resource_File.resx"));
            if (xml.Root == null)
                throw new Exception("Invalid empty resource file.");

            foreach (var resource in resources)
            {
                xml.Root.Add(new XElement("data",
                    new XAttribute("name", resource.Key),
                    new XAttribute(XNamespace.Xml + "space", "preserve"),
                    new XElement("value", resource.Value)));
            }

            var tempPath = Path.GetTempFileName().Replace(".tmp", "." + Program.Configuration.Locale + ".resx");
            xml.Save(tempPath);

            return tempPath;
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
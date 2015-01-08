using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;

namespace Saxx.LingoHubSyncer.Resx
{
    public class CombineResxService
    {
        #region Constructor

        private readonly ResxUtility _resxService;


        public CombineResxService()
        {
            _resxService = new ResxUtility();
        }

        #endregion

        public string CombineDirectoryToSingleResourceFile(string path)
        {
            var resources = new Dictionary<string, Phrase>();

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


        private void AddResourcesFromDirectoryRecursive(string directoryPath, string basePath, IDictionary<string, Phrase> resources)
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


        private void AddResourcesFromFile(string filePath, string basePath, IDictionary<string, Phrase> resources)
        {
            var xml = XDocument.Load(filePath);
            if (xml.Root == null)
                return;

            foreach (var element in xml.Root.Elements("data"))
            {
                var key = _resxService.EncodePathInResourceKey(filePath, basePath, element.Attribute("name").Value);

                var phrase = new Phrase(element);
                if (phrase.Value == null)
                    continue;

                resources[key] = phrase;
            }
        }


        // ReSharper disable once ParameterTypeCanBeEnumerable.Local
        private string WriteToTemporaryResxFile(IDictionary<string, Phrase> resources)
        {
            var xml = XDocument.Load(Path.Combine(Program.GetWorkingDirectory(), "Empty_Resource_File.resx"));
            if (xml.Root == null)
                throw new Exception("Invalid empty resource file.");

            foreach (var keyValuePair in resources)
            {
                xml.Root.Add(new XElement("data",
                    new XAttribute("name", keyValuePair.Key),
                    new XAttribute(XNamespace.Xml + "space", "preserve"),
                    new XElement("value", keyValuePair.Value.Value),
                    string.IsNullOrEmpty(keyValuePair.Value.Comment) ? null : new XElement("comment", keyValuePair.Value.Comment)));
            }

            var tempPath = Path.GetTempFileName().Replace(".tmp", "." + Program.Configuration.Locale + ".resx");
            xml.Save(tempPath);

            return tempPath;
        }



    }
}
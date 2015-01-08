using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace Saxx.LingoHubSyncer.Resx
{
    public class ExpandResxService
    {
        #region Constructor

        private readonly ResxUtility _resxService;


        public ExpandResxService()
        {
            _resxService = new ResxUtility();
        }

        #endregion

        public void ExpandSingleResourceFileToDirectory(string directoryPath, string resxFilePath)
        {
            var resources = new Dictionary<string, Phrase>();

            var xml = XDocument.Load(resxFilePath);
            if (xml.Root == null)
                return;

            foreach (var element in xml.Root.Elements("data"))
            {
                var key = element.Attribute("name").Value;

                var phrase = new Phrase(element);
                if (phrase.Value == null)
                    continue;

                resources[key] = phrase;
            }

            var fileNames = resources.Keys.Select(x => _resxService.DecodePathInResoureKey(directoryPath, Program.Configuration.Locale, x)).Distinct();
            foreach (var fileName in fileNames)
                CreateEmptyFile(fileName);

            foreach (var keyValuePair in resources)
            {
                AddToFile(directoryPath, keyValuePair.Key, keyValuePair.Value.Value, keyValuePair.Value.Comment);
            }
        }


        private void CreateEmptyFile(string filePath)
        {
            var directoryPath = Path.GetDirectoryName(filePath);
            if (string.IsNullOrEmpty(directoryPath))
                return;

            if (!Directory.Exists(directoryPath))
                Directory.CreateDirectory(directoryPath);

            if (File.Exists(filePath))
                File.Delete(filePath);

            File.Copy(Path.Combine(Program.GetWorkingDirectory(), "Empty_Resource_File.resx"), filePath);
        }


        private void AddToFile(string baseDirectoryPath, string encodedKey, string value, string comment)
        {
            string filePath;
            string key;

            _resxService.DecodePathInResoureKey(baseDirectoryPath, Program.Configuration.Locale, encodedKey, out filePath, out key);

            // if this is not the source locale, and the value is empty, don't write the empty value to the file (so that the default resx is used)
            if (string.IsNullOrEmpty(value) && _resxService.GetLocale(filePath) != null)
                return;

            var xml = XDocument.Load(filePath);
            if (xml.Root == null)
                return;

            xml.Root.Add(new XElement("data",
                new XAttribute("name", key),
                new XAttribute(XNamespace.Xml + "space", "preserve"),
                new XElement("value", value),
                string.IsNullOrEmpty(comment) ? null : new XElement("comment", comment)));

            xml.Save(filePath);
        }
    }
}

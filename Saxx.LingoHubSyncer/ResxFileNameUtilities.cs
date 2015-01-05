using System;
using System.IO;

namespace Saxx.LingoHubSyncer
{
    public class ResxFileNameUtilities
    {
        public string EncodePathInResourceKey(string filePath, string basePath, string resourceKey)
        {
            if (string.IsNullOrEmpty(filePath))
                throw new ArgumentNullException(filePath);
            if (string.IsNullOrEmpty(basePath))
                throw new ArgumentNullException(basePath);

            // ReSharper disable PossibleNullReferenceException
            if (File.Exists(basePath))
                basePath = Path.GetDirectoryName(basePath);
            filePath = filePath.Substring(basePath.Length);

            var fileNameWithoutLocaleAndExtension = Path.GetFileNameWithoutExtension(filePath);
            var locale = GetLocale(filePath);
            if (locale != null)
                fileNameWithoutLocaleAndExtension = fileNameWithoutLocaleAndExtension.Substring(0, fileNameWithoutLocaleAndExtension.Length - locale.Length - 1);

            var result = Path.GetDirectoryName(filePath).Replace(Path.DirectorySeparatorChar, '/').Trim('/');
            result += "/" + fileNameWithoutLocaleAndExtension;
            result += "/" + resourceKey;
            result = result.Trim('/');
            // ReSharper restore PossibleNullReferenceException

            return result;
        }


        public string GetLocale(string fileName)
        {
            fileName = Path.GetFileNameWithoutExtension(fileName);

            var locale = (Path.GetExtension(fileName) ?? "").Trim('.');
            if (!string.IsNullOrEmpty(locale))
            {
                if (locale.Length == 2)
                    return locale;
                if (locale.Length == 5 && locale[3] == '-')
                    return locale;
            }

            return null;
        }
    }
}

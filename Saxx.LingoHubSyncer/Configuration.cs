using CommandLine;
using CommandLine.Text;

namespace Saxx.LingoHubSyncer
{
    public class Configuration
    {
        public Configuration()
        {
            DefaultLocale = "en";
            Locale = "en";
        }


        [Option('u', "username", Required = true, HelpText = "LingoHub username.")]
        public string Username { get; set; }

        [Option('p', "password", Required = true, HelpText = "LingoHub password.")]
        public string Password { get; set; }

        [Option('m', "mode", Required = true, HelpText = "Either 'Upload' or 'Download'.")]
        public Modes Mode { get; set; }

        [Option("project", Required = true, HelpText = "LingoHub project title.")]
        public string Project { get; set; }

        [Option("path", HelpText = "Path to a resource file to upload to, or directory to download to. " +
                                   "If this is a directory and the 'Mode' is 'Upload', all resource files in the directory will be uploaded recursively.")]
        public string Path { get; set; }

        [Option("defaultLocale", HelpText = "The locale for resource files that without specific locale in filename. " +
                                            "Default value is 'en'.")]
        public string DefaultLocale { get; set; }

        [Option("locale", HelpText = "Only upload or download resources of a specific locale. " +
                                     "Does not apply when uploading a single file." +
                                     "Default value is 'en'.")]
        public string Locale { get; set; }


        [HelpOption]
        public string GetUsage()
        {
            return HelpText.AutoBuild(this, current => HelpText.DefaultParsingErrorsHandler(this, current));
        }


        public enum Modes
        {
            Upload,
            Download
        }
    }
}
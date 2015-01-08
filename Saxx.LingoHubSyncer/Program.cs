using System;
using System.IO;
using CommandLine;

namespace Saxx.LingoHubSyncer
{
    internal class Program
    {
        internal static Configuration Configuration { get; set; }


        private static void Main(string[] args)
        {
            Configuration = new Configuration();
            if (Parser.Default.ParseArguments(args, Configuration))
            {
                try
                {
                    switch (Configuration.Mode)
                    {
                        case Configuration.Modes.Upload:
                        {
                            using (var uploader = new UploadService())
                                uploader.Run();
                            break;
                        }
                        case Configuration.Modes.Download:
                        {
                            using (var downloader = new DownloadService())
                                downloader.Run();
                            break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    Console.Error.WriteLine(ex.ToString());
                    throw;
                }
            }
        }


        internal static string GetWorkingDirectory()
        {
            // we need this fancy way to make it work in NUnit, because the DLL is executed from some temp path
            var codeBase = typeof(Program).Assembly.EscapedCodeBase;
            var uri = new UriBuilder(codeBase);
            var path = Uri.UnescapeDataString(uri.Path);
            return Path.GetDirectoryName(path);
        }
    }
}
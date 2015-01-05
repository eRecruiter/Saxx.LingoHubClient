using System;

namespace Saxx.LingoHubSyncer
{
    internal class Program
    {
        internal static Configuration Configuration { get; set; }


        private static void Main(string[] args)
        {
            Configuration = new Configuration();
            if (CommandLine.Parser.Default.ParseArguments(args, Configuration))
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
    }
}
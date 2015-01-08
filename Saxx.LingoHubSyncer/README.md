Saxx.LingoHubClientSyncer
=========================

This is a .NET/C# command line application to sync resource files to [LingoHub](http://lingohub.com) and back. This works for single files, but also for entire directory trees, which is especially useful if you need to sync and ASP.NET application with a lot of RESX files in `App_LocalResources`. At the moment only RESX files are supported.

You can download [the latest version](https://ci.appveyor.com/project/saxx/saxx-lingohubclient/build/artifacts). This version is built automatically [![Build status](https://ci.appveyor.com/api/projects/status/tvqo9y7lhv86d08c?svg=true)](https://ci.appveyor.com/project/saxx/saxx-lingohubclient)

See `Saxx.LingoHubSyncer --help` for command line options. A typical call usually looks something like:

`Saxx.LingoHubSyncer.exe -u <LingoHub_Username> -p <LingoHub_Password> -m Upload --project <LingoHub_Project> --path <Path_to_Applicationor_Resx_File> --locale de --defaultLocale en`

and

`Saxx.LingoHubSyncer.exe -u <LingoHub_Username> -p <LingoHub_Password> -m Download --project <LingoHub_Project> --path <Path_to_Application_or_Resx_File> --locale de --defaultLocale en`


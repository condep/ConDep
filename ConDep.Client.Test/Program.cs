using System;
using ConDep.Node.Client;

namespace ConDep.Client.Test
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            SyncWebApp();
            //SyncFile();
            //SyncDir();
        }

        private static void SyncDir()
        {
            var api = new Api("http://jat-web03/ConDepNode/", "", "");
            var srcRoot = @"C:\GitHub\ConDep\Build";
            var dstRoot = @"C:\temp\ConDep\Build";
            var result = api.SyncDir(srcRoot, dstRoot);

            Console.WriteLine(
                @"Sync result:

    Files Created       : {0}
    Files Updated       : {3}
    Files Deleted       : {2}
    Directories Deleted : {1}
", result.CreatedFiles, result.DeletedDirectories, result.DeletedFiles, result.UpdatedFiles);
        }

        private static void SyncFile()
        {
            var api = new Api("http://localhost/ConDepNode/", "", "");
            var srcRoot = @"C:\GitHub\ConDep\Build\ConDep.Dsl.nuspec";
            var dstRoot = @"C:\temp\ConDep\Build\ConDep.Dsl.nuspec";
            var result = api.SyncFile(srcRoot, dstRoot);

            Console.WriteLine(
                @"Sync result:

    Files Created       : {0}
    Files Updated       : {3}
    Files Deleted       : {2}
    Directories Deleted : {1}
", result.CreatedFiles, result.DeletedDirectories, result.DeletedFiles, result.UpdatedFiles);
        }

        private static void SyncWebApp()
        {
            var api = new Api("http://localhost/ConDepNode/", "", "");
            var srcRoot = @"C:\GitHub\ConDep\Build";
            var dstRoot = @"C:\temp\ConDep\Build";
            var result = api.SyncWebApp("Default Web Site", "WebAppConDepTest", srcRoot, dstRoot);

            Console.WriteLine(
                @"Sync result:

    Files Created       : {0}
    Files Updated       : {3}
    Files Deleted       : {2}
    Directories Deleted : {1}
", result.CreatedFiles, result.DeletedDirectories, result.DeletedFiles, result.UpdatedFiles);
        }
    }
}

using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ConDep.Dsl.Remote.Node.Model
{
    public static class DirectoryInfoExtensions
    {
        public static IEnumerable<SyncPath> GetAllRelativeFilePaths(this DirectoryInfo dirInfo)
        {
            return GetAllRelativeFilePaths(dirInfo, dirInfo.FullName);
        }

        private static IEnumerable<SyncPath> GetAllRelativeFilePaths(this DirectoryInfo dirInfo, string rootPath, bool isRoot = true)
        {
            var paths = new List<SyncPath>();
            if(!isRoot)
            {
                var path = new SyncPath(dirInfo.FullName, rootPath, dirInfo.Attributes.ToString(), true);
                paths.Add(path);
            }

            paths.AddRange(dirInfo.GetFiles().Select(file => new SyncPath(file.FullName, rootPath, file.Attributes.ToString())));
            paths.AddRange(dirInfo.GetDirectories().SelectMany(dir => GetAllRelativeFilePaths(dir, rootPath, false)));
            return paths;
        } 

        public static string TrimPath(this FileInfo fileInfo, string rootPathToRemove)
        {
            if(!rootPathToRemove.EndsWith("\\"))
                rootPathToRemove += "\\";

            return fileInfo.FullName.Replace(rootPathToRemove, "");
        }

        public static string TrimPath(this DirectoryInfo dirInfo, string rootPathToRemove)
        {
            if (!rootPathToRemove.EndsWith("\\"))
                rootPathToRemove += "\\";

            return dirInfo.FullName.Replace(rootPathToRemove, "");
        }
    }
}
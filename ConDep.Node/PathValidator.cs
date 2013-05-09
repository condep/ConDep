using System;
using System.IO;

namespace ConDep.Node
{
    public class PathValidator
    {
        public bool ValidPath(string dirPath)
        {
            var dirInfo = new DirectoryInfo(dirPath);
            if (dirInfo.Parent == null) return false; //Don't allow a Root path - e.g C:\

            //Don't allow the most common system folders like Windows, System32 and Program Files
            if (dirInfo.FullName == Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86)) return false;
            if (dirInfo.FullName == Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles)) return false;
            if (dirInfo.FullName == Environment.GetFolderPath(Environment.SpecialFolder.Windows)) return false;
            if (dirInfo.FullName == Environment.GetFolderPath(Environment.SpecialFolder.System)) return false;
            if (dirInfo.FullName == Environment.GetFolderPath(Environment.SpecialFolder.SystemX86)) return false;

            return true;
        }
    }
}
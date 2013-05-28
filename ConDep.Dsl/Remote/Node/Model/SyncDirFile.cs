using System;
using System.Collections.Generic;
using System.IO;

namespace ConDep.Dsl.Remote.Node.Model
{
    public class SyncDirFile
    {
        private readonly List<Link> _links = new List<Link>();

        public string FullPath { get; set; }
        public string RelativePath { get; set; }
        public DateTime LastWriteTimeUtc { get; set; }
        public long Size { get; set; }
        public string Attributes { get; set; }
        public List<Link> Links { get { return _links; } }

        public string TrimPath(string rootPathToRemove)
        {
            if (!rootPathToRemove.EndsWith("\\"))
                rootPathToRemove += "\\";

            return FullPath.Replace(rootPathToRemove, "");
        }

        public bool EqualTo(FileInfo other, string fileInfoRootPath)
        {
            if (other == null) return false;
            if (RelativePath != other.TrimPath(fileInfoRootPath)) return false;
            if (LastWriteTimeUtc != other.LastWriteTimeUtc) return false;
            if (Size != other.Length) return false;
            if (Attributes != other.Attributes.ToString()) return false;
            return true;
        }
    }
}
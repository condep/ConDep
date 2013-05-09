using System.Collections.Generic;

namespace ConDep.Node.Model
{
    public class SyncDirDirectory
    {
        private readonly List<SyncDirFile> _files = new List<SyncDirFile>();
        private readonly List<SyncDirDirectory> _dirs = new List<SyncDirDirectory>();
        private readonly List<Link> _links = new List<Link>();

        public string FullPath { get; set; }
        public string RelativePath { get; set; }

        public string Attributes { get; set; }
        public List<SyncDirFile> Files { get { return _files; } }
        public List<SyncDirDirectory> Directories { get { return _dirs; } }
        public List<Link> Links { get { return _links; } }
    }
}
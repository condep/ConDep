using System;
using System.Collections.Generic;

namespace ConDep.Node.Model
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
    }
}
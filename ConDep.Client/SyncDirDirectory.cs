using System.Collections.Generic;
using System.Linq;

namespace ConDep.Client
{
    public class SyncDirDirectory
    {
        private readonly List<SyncDirFile> _files = new List<SyncDirFile>();
        private readonly List<SyncDirDirectory> _dirs = new List<SyncDirDirectory>();

        public string Path { get; set; }
        public string Attributes { get; set; }
        public List<SyncDirFile> Files { get { return _files; } }
        public List<SyncDirDirectory> Directories { get { return _dirs; } }
        public JsonLink Link { get; set; }

        public SyncDirFile GetByRelativePath(string relativePath)
        {
            foreach (var file in Files.Where(file => file.Path.EndsWith(relativePath)))
            {
                return file;
            }

            return Directories.Select(dir => dir.GetByRelativePath(relativePath)).FirstOrDefault(file => file != null);
        }
    }
}
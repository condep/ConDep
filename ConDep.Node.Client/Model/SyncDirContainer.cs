using System.Collections.Generic;

namespace ConDep.Node.Client.Model
{
    public class SyncDirContainer
    {
        private readonly List<string> _filesToCopy = new List<string>();
        private readonly List<string> _filesToDelete = new List<string>();
        private readonly List<string> _filesToUpdate = new List<string>();

        public List<string> FilesToCopy { get { return _filesToCopy; } }
        public List<string> FilesToDelete { get { return _filesToDelete; } }
        public List<string> FilesToUpdate { get { return _filesToUpdate; } }
    }
}
using System.Collections.Generic;

namespace ConDep.Dsl.Remote.Node.Model
{
    public class SyncResult
    {
        private readonly List<string> _deletedFiles = new List<string>();
        private readonly List<string> _deletedDirectories = new List<string>();
        private readonly List<string> _updatedFiles = new List<string>();
        private readonly List<string> _createdFiles = new List<string>();
        private readonly List<string> _log = new List<string>();

        public List<string> DeletedFiles { get { return _deletedFiles; } }
        public List<string> DeletedDirectories { get { return _deletedDirectories; } }
        public List<string> UpdatedFiles { get { return _updatedFiles; } }
        public List<string> CreatedFiles { get { return _createdFiles; } }
        public List<string> Log { get { return _log; } }
    }
}
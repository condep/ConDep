using System.Collections.Generic;

namespace ConDep.Node.Model
{
    public class SyncPostProcContent
    {
        public IEnumerable<SyncPath> DeletedFiles { get; set; }
        public IEnumerable<SyncPath> ChangedDirectories { get; set; }
        public IEnumerable<SyncPath> DeletedDirectories { get; set; }
    }
}
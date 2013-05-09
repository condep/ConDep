using System.Collections.Generic;
using ConDep.Node.Client.Model;

namespace ConDep.Node.Client
{
    public class SyncPostProcContent
    {
        public IEnumerable<SyncPath> DeletedFiles { get; set; }
        public IEnumerable<SyncPath> ChangedDirectories { get; set; }
        public IEnumerable<SyncPath> DeletedDirectories { get; set; }
    }
}
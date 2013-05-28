using System.Collections.Generic;
using ConDep.Dsl.Remote.Node.Model;

namespace ConDep.Dsl.Remote.Node
{
    public class SyncPostProcContent
    {
        public IEnumerable<SyncPath> DeletedFiles { get; set; }
        public IEnumerable<SyncPath> ChangedDirectories { get; set; }
        public IEnumerable<SyncPath> DeletedDirectories { get; set; }
    }
}
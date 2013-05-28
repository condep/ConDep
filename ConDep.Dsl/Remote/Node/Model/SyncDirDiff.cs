using System.Linq;
using System.Collections.Generic;

namespace ConDep.Dsl.Remote.Node.Model
{
    public class SyncDirDiff
    {
        public IEnumerable<SyncPath> MissingPaths { get; set; }
        public IEnumerable<SyncPath> ChangedPaths { get; set; }
        public IEnumerable<SyncPath> DeletedPaths { get; set; }
        public IEnumerable<SyncPath> DeletedFiles { get { return DeletedPaths.Where(x => x.IsDirectory == false); } }
        public IEnumerable<SyncPath> DeletedDirectories { get { return DeletedPaths.Where(x => x.IsDirectory); } }
        public IEnumerable<SyncPath> MissingAndChangedPaths
        {
            get
            {
                if(MissingPaths == null && ChangedPaths == null) return new List<SyncPath>();
                if (MissingPaths == null && ChangedPaths != null) return ChangedPaths;
                if (MissingPaths != null && ChangedPaths == null) return MissingPaths;

                return MissingPaths.Concat(ChangedPaths);
            }
        } 

        public IEnumerable<SyncPath> ChangedDirectories
        {
            get { return ChangedPaths.Where(x => x.IsDirectory); }
        } 
    }
}
using System.Collections.Generic;

namespace ConDep.Node.Client.Model
{
    public class SyncPathRelativePathComparer : IEqualityComparer<SyncPath>
    {
        public bool Equals(SyncPath x, SyncPath y)
        {
            return x.RelativePath.Equals(y.RelativePath);
        }

        public int GetHashCode(SyncPath obj)
        {
            return obj.RelativePath == null ? 0 : obj.RelativePath.GetHashCode();
        }
    }
}
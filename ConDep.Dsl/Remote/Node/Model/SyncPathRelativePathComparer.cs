using System.Collections.Generic;

namespace ConDep.Dsl.Remote.Node.Model
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
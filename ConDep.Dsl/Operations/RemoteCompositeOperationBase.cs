using System.Collections.Generic;
using System.IO;
using ConDep.Dsl.SemanticModel;

namespace ConDep.Dsl.Operations
{
    public abstract class RemoteCompositeOperationBase
    {
        public void ConfigureRemoteScripts(IOfferRemoteOperations server, IEnumerable<string> scripts)
        {
            foreach (var script in scripts)
            {
                server.Deploy.File(script, @"%temp%\ConDepPSScripts\" + Path.GetFileName(script));
            }
        }

        public abstract string Name { get; }
        public abstract bool IsValid(Notification notification);
    }
}
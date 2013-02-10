using System.Collections.Generic;
using System.IO;
using ConDep.Dsl.Builders;
using ConDep.Dsl.SemanticModel;

namespace ConDep.Dsl.Operations
{
    public abstract class RemoteCompositeOperation : RemoteCompositeOperationBase
    {
        public abstract void Configure(IOfferRemoteComposition server);
    }

    public abstract class RemoteCompositeInfrastructureOperation : RemoteCompositeOperationBase {
        public abstract void Configure(IOfferRemoteComposition server, IOfferInfrastructure require);
    }

    public abstract class RemoteCompositeOperationBase
    {
        public void ConfigureRemoteScripts(IOfferRemoteOperations server, IEnumerable<string> scripts)
        {
            foreach (var script in scripts)
            {
                server.Deploy.File(script, @"%temp%\ConDepPSScripts\" + Path.GetFileName(script));
            }
        }

        //protected string SourcePath { get; set; }
        //protected virtual string DestinationPath { get; set; }

        public abstract string Name { get; }
        public abstract bool IsValid(Notification notification);
    }

}
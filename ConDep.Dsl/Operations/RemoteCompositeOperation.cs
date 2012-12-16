using ConDep.Dsl.Builders;
using ConDep.Dsl.SemanticModel;

namespace ConDep.Dsl.Operations
{
    public abstract class RemoteCompositeOperation
    {
        public abstract void Configure(IOfferRemoteComposition server);
        public string SourcePath { get; set; }
        public virtual string DestinationPath { get; set; }
        public abstract bool IsValid(Notification notification);
    }
}
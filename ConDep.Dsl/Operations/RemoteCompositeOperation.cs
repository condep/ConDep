using ConDep.Dsl.Builders;
using ConDep.Dsl.SemanticModel;

namespace ConDep.Dsl.Operations
{
    public abstract class RemoteCompositeOperation
    {
        public abstract void Configure(IOfferRemoteComposition server);
        protected string SourcePath { get; set; }
        protected virtual string DestinationPath { get; set; }
        public abstract bool IsValid(Notification notification);
    }
}
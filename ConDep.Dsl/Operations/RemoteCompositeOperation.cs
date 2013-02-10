using ConDep.Dsl.Builders;

namespace ConDep.Dsl.Operations
{
    public abstract class RemoteCompositeOperation : RemoteCompositeOperationBase
    {
        public abstract void Configure(IOfferRemoteComposition server);
    }
}
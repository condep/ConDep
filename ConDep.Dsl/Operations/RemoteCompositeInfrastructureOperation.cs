namespace ConDep.Dsl.Operations
{
    public abstract class RemoteCompositeInfrastructureOperation : RemoteCompositeOperationBase {
        public abstract void Configure(IOfferRemoteComposition server, IOfferInfrastructure require);
    }
}
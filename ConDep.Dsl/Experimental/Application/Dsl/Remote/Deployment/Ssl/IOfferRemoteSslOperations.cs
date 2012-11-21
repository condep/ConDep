namespace ConDep.Dsl.Experimental.Application
{
    public interface IOfferRemoteSslOperations
    {
        IOfferRemoteDeployment FromStore();
        IOfferRemoteDeployment FromFile();
    }
}
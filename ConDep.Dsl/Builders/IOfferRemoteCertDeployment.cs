namespace ConDep.Dsl.Builders
{
    public interface IOfferRemoteCertDeployment
    {
        IOfferRemoteDeployment FromStore();
        IOfferRemoteDeployment FromFile();
    }
}
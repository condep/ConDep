namespace ConDep.Dsl.Builders
{
    public interface IOfferRemoteCertDeployment
    {
        IOfferRemoteDeployment FromStore(string thumbprint);
        IOfferRemoteDeployment FromFile(string path, string password);
    }
}
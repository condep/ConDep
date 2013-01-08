using System.Security.Cryptography.X509Certificates;

namespace ConDep.Dsl.Builders
{
    public interface IOfferRemoteCertDeployment
    {
        IOfferRemoteDeployment FromStore(string thumbprint);
        IOfferRemoteDeployment FromFile(string path, string password);
        IOfferRemoteDeployment FromStore(X509FindType findType, string findValue);
    }
}
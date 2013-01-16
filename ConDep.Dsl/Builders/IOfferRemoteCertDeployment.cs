using System;
using System.Security.Cryptography.X509Certificates;
using ConDep.Dsl.Operations.Infrastructure.IIS.WebSite;

namespace ConDep.Dsl.Builders
{
    public interface IOfferRemoteCertDeployment
    {
        IOfferRemoteDeployment FromStore(X509FindType findType, string findValue, Action<IOfferCertificateOptions> options = null);
        IOfferRemoteDeployment FromFile(string path, string password, Action<IOfferCertificateOptions> options = null);
    }
}
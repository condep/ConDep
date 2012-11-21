using System;
using ConDep.Dsl.WebDeployProviders.Deployment.NServiceBus;

namespace ConDep.Dsl.Experimental.Application
{
    public interface IOfferRemoteDeployment
    {
        IOfferRemoteDeployment Directory(string sourceDir, string destDir);
        IOfferRemoteDeployment File(string sourceFile, string destFile);
        IOfferRemoteDeployment IisWebApplication();
        IOfferRemoteDeployment WindowsService();
        IOfferRemoteDeployment NServiceBusEndpoint(string sourceDir, string destDir, string serviceName);
        IOfferRemoteDeployment NServiceBusEndpoint(string sourceDir, string destDir, string serviceName, Action<NServiceBusOptions> nServiceBusOptions);
        IOfferRemoteSslOperations SslCertificate { get; }
    }
}
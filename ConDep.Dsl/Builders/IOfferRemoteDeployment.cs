using System;
using ConDep.Dsl.Operations.Application.Deployment.NServiceBus;

namespace ConDep.Dsl.Builders
{
    public interface IOfferRemoteDeployment
    {
        IOfferRemoteDeployment Directory(string sourceDir, string destDir);
        IOfferRemoteDeployment File(string sourceFile, string destFile);
        IOfferRemoteDeployment IisWebApplication(string sourceDir, string webAppName, string webSiteName);
        IOfferRemoteDeployment WindowsService(string serviceName, string sourceDir, string destDir, string relativeExePath, string displayName);
        IOfferRemoteDeployment WindowsService(string serviceName, string sourceDir, string destDir, string relativeExePath, string displayName, Action<IOfferWindowsServiceOptions> options);

        IOfferRemoteDeployment NServiceBusEndpoint(string sourceDir, string destDir, string serviceName, Action<NServiceBusOptions> nServiceBusOptions = null);
        IOfferRemoteCertDeployment SslCertificate { get; }
    }
}
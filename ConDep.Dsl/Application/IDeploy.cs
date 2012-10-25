using System;
using ConDep.Dsl.WebDeployProviders.Deployment.NServiceBus;

namespace ConDep.Dsl.Application
{
    public interface IDeploy
    {
        void Directory(string sourceDir, string destDir);
        void File(string sourceFile, string destFile);
        void IisWebApplication();
        void WindowsService();
        void NServiceBusEndpoint(string sourceDir, string destDir, string serviceName);
        void NServiceBusEndpoint(string sourceDir, string destDir, string serviceName, Action<NServiceBusOptions> nServiceBusOptions);
        IDeploySslCertificate SslCertificate { get; }
    }
}
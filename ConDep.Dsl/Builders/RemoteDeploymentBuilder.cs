using System;
using ConDep.Dsl.Operations;
using ConDep.Dsl.Operations.Application.Deployment.CopyDir;
using ConDep.Dsl.Operations.Application.Deployment.CopyFile;
using ConDep.Dsl.Operations.Application.Deployment.NServiceBus;
using ConDep.Dsl.Operations.Application.Deployment.WebApp;
using ConDep.Dsl.Operations.Application.Deployment.WindowsService;
using ConDep.Dsl.SemanticModel;
using ConDep.Dsl.SemanticModel.Sequence;
using ConDep.Dsl.SemanticModel.WebDeploy;

namespace ConDep.Dsl.Builders
{
    public class RemoteDeploymentBuilder : IOfferRemoteDeployment, IConfigureRemoteDeployment
    {
        private readonly IManageRemoteSequence _remoteSequence;
        private readonly IHandleWebDeploy _webDeploy;

        public RemoteDeploymentBuilder(IManageRemoteSequence remoteSequence, IHandleWebDeploy webDeploy)
        {
            _remoteSequence = remoteSequence;
            _webDeploy = webDeploy;
        }

        public IOfferRemoteDeployment Directory(string sourceDir, string destDir)
        {
            var copyDirProvider = new CopyDirProvider(sourceDir, destDir);
            AddOperation(copyDirProvider);
            return this;
        }

        public IOfferRemoteDeployment File(string sourceFile, string destFile)
        {
            var copyFileProvider = new CopyFileProvider(sourceFile, destFile);
            AddOperation(copyFileProvider);
            return this;
        }

        public IOfferRemoteDeployment IisWebApplication(string sourceDir, string webAppName, string webSiteName)
        {
            var webAppProvider = new WebAppDeploymentProvider(sourceDir, webAppName, webSiteName);
            AddOperation(webAppProvider);
            return this;
        }

        public IOfferRemoteDeployment WindowsService(string serviceName, string sourceDir, string destDir, string relativeExePath, string displayName)
        {
            var winServiceOperation = new WindowsServiceOperation(serviceName, sourceDir, destDir, relativeExePath, displayName);
            AddOperation(winServiceOperation);
            return this;
        }

        public IOfferRemoteDeployment WindowsService(string serviceName, string sourceDir, string destDir, string relativeExePath, string displayName, Action<IOfferWindowsServiceOptions> options)
        {
            var winServiceOptions = new WindowsServiceOptions();
            options(winServiceOptions);

            var winServiceOperation = new WindowsServiceOperation(serviceName, sourceDir, destDir, relativeExePath, displayName, winServiceOptions.Values);
            AddOperation(winServiceOperation);
            return this;
        }

        public IOfferRemoteDeployment NServiceBusEndpoint(string sourceDir, string destDir, string serviceName)
        {
            return NServiceBusEndpoint(sourceDir, destDir, serviceName, null);
        }

        public IOfferRemoteDeployment NServiceBusEndpoint(string sourceDir, string destDir, string serviceName, Action<IOfferNServiceBusOptions> nServiceBusOptions)
        {
            var nServiceBusProvider = new NServiceBusOperation(sourceDir, destDir, serviceName);
            if (nServiceBusOptions != null)
            {
                nServiceBusOptions(new NServiceBusOptions(nServiceBusProvider));
            }
            AddOperation(nServiceBusProvider);
            return this;
        }

        public IOfferRemoteCertDeployment SslCertificate { get { return new RemoteCertDeploymentBuilder(_remoteSequence, _webDeploy, this); } }

        public void AddOperation(RemoteCompositeOperation operation)
        {
            operation.Configure(new RemoteCompositeBuilder(_remoteSequence.NewCompositeSequence(operation), _webDeploy));
        }

        public void AddOperation(WebDeployProviderBase provider)
        {
            _remoteSequence.Add(new RemoteWebDeployOperation(provider, _webDeploy));
        }
    }
}
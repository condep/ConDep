using System;
using ConDep.Dsl.Operations.Application.Deployment.CopyDir;
using ConDep.Dsl.Operations.Application.Deployment.CopyFile;
using ConDep.Dsl.Operations.Application.Deployment.NServiceBus;
using ConDep.Dsl.Operations.Application.Deployment.WebApp;
using ConDep.Dsl.SemanticModel;
using ConDep.Dsl.SemanticModel.Sequence;
using ConDep.Dsl.SemanticModel.WebDeploy;

namespace ConDep.Dsl.Builders
{
    public class RemoteDeploymentBuilder : IOfferRemoteDeployment
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
            _remoteSequence.Add(new RemoteWebDeployOperation(copyDirProvider, _webDeploy));
            return this;
        }

        public IOfferRemoteDeployment File(string sourceFile, string destFile)
        {
            var copyFileProvider = new CopyFileProvider(sourceFile, destFile);
            _remoteSequence.Add(new RemoteWebDeployOperation(copyFileProvider, _webDeploy));
            return this;
        }

        public IOfferRemoteDeployment IisWebApplication(string sourceDir, string webAppName, string webSiteName)
        {
            var webAppProvider = new WebAppDeploymentProvider(sourceDir, webAppName, webSiteName);
            _remoteSequence.Add(new RemoteWebDeployOperation(webAppProvider, _webDeploy));
            return this;
        }

        public IOfferRemoteDeployment WindowsService()
        {
            throw new System.NotImplementedException();
        }

        public IOfferRemoteDeployment NServiceBusEndpoint(string sourceDir, string destDir, string serviceName)
        {
            var nServiceBusProvider = new NServiceBusOperation(sourceDir, destDir, serviceName);
            nServiceBusProvider.Configure(new RemoteCompositeBuilder(_remoteSequence.NewCompositeSequence("NServiceBus"), _webDeploy));
            return this;
        }

        public IOfferRemoteDeployment NServiceBusEndpoint(string sourceDir, string destDir, string serviceName, Action<NServiceBusOptions> nServiceBusOptions)
        {
            var nServiceBusProvider = new NServiceBusOperation(sourceDir, destDir, serviceName);
            nServiceBusOptions(new NServiceBusOptions(nServiceBusProvider));
            nServiceBusProvider.Configure(new RemoteCompositeBuilder(_remoteSequence.NewCompositeSequence("NServiceBus"), _webDeploy));
            return this;
        }

        public IOfferRemoteCertDeployment SslCertificate { get { return new RemoteCertDeploymentBuilder(_remoteSequence, _webDeploy, this); } }
    }
}
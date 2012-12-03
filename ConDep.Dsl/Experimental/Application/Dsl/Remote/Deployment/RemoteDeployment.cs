using System;
using ConDep.Dsl.Experimental.Application.Dsl.Remote;
using ConDep.Dsl.Experimental.Core;
using ConDep.Dsl.WebDeployProviders.Deployment.CopyDir;
using ConDep.Dsl.WebDeployProviders.Deployment.CopyFile;
using ConDep.Dsl.WebDeployProviders.Deployment.IIS.WebApp;
using ConDep.Dsl.WebDeployProviders.Deployment.NServiceBus;

namespace ConDep.Dsl.Experimental.Application.Deployment
{
    public class RemoteDeployment : IOfferRemoteDeployment
    {
        private readonly IManageRemoteSequence _remoteSequence;
        private readonly IOfferRemoteSslOperations _sslCertDeployment;
        private readonly IOperateWebDeploy _webDeploy;
        private readonly ILogForConDep _logger;
        private readonly RemoteServerOffer _remoteServerOffer;

        public RemoteDeployment(IManageRemoteSequence remoteSequence, IOfferRemoteSslOperations sslCertDeployment, IOperateWebDeploy webDeploy, ILogForConDep logger, RemoteServerOffer remoteServerOffer)
        {
            _remoteSequence = remoteSequence;
            _sslCertDeployment = sslCertDeployment;
            _webDeploy = webDeploy;
            _logger = logger;
            _remoteServerOffer = remoteServerOffer;
        }

        public IOfferRemoteDeployment Directory(string sourceDir, string destDir)
        {
            var copyDirProvider = new CopyDirProvider(sourceDir, destDir);
            _remoteSequence.Add(new RemoteOperation(copyDirProvider, _logger, _webDeploy));
            return this;
        }

        public IOfferRemoteDeployment File(string sourceFile, string destFile)
        {
            var copyFileProvider = new CopyFileProvider(sourceFile, destFile);
            _remoteSequence.Add(new RemoteOperation(copyFileProvider, _logger, _webDeploy));
            return this;
        }

        public IOfferRemoteDeployment IisWebApplication(string sourceDir, string webAppName, string webSiteName)
        {
            var webAppProvider = new WebAppDeploymentProvider(sourceDir, webAppName, webSiteName);
            _remoteSequence.Add(new RemoteOperation(webAppProvider));
            return this;
        }

        public IOfferRemoteDeployment WindowsService()
        {
            throw new System.NotImplementedException();
        }

        public IOfferRemoteDeployment NServiceBusEndpoint(string sourceDir, string destDir, string serviceName)
        {
            var nServiceBusProvider = new NServiceBusOperation(sourceDir, destDir, serviceName);
            nServiceBusProvider.Configure(_remoteServerOffer);
            return this;
        }

        public IOfferRemoteDeployment NServiceBusEndpoint(string sourceDir, string destDir, string serviceName, Action<NServiceBusOptions> nServiceBusOptions)
        {
            var nServiceBusProvider = new NServiceBusOperation(sourceDir, destDir, serviceName);
            nServiceBusOptions(new NServiceBusOptions(nServiceBusProvider));
            nServiceBusProvider.Configure(_remoteServerOffer);
            return this;
        }

        public IOfferRemoteSslOperations SslCertificate { get { return _sslCertDeployment; } }

        public IManageRemoteSequence Sequence { get { return _remoteSequence; } }
    }
}
using System;
using ConDep.Dsl.Operations;
using ConDep.Dsl.Operations.Application.Deployment.CopyDir;
using ConDep.Dsl.Operations.Application.Deployment.CopyFile;
using ConDep.Dsl.Operations.Application.Deployment.NServiceBus;
using ConDep.Dsl.Operations.Application.Deployment.WebApp;
using ConDep.Dsl.Operations.Application.Deployment.WindowsService;
using ConDep.Dsl.Operations.Application.Local;
using ConDep.Dsl.SemanticModel;
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
            //var copyDirProvider = new CopyDirProvider(sourceDir, destDir);
            //AddOperation(copyDirProvider);
            var copyDirOperation = new CopyDirOperation(sourceDir, destDir);
            AddOperation(copyDirOperation);
            return this;
        }

        public IOfferRemoteDeployment File(string sourceFile, string destFile)
        {
            //var copyFileProvider = new CopyFileProvider(sourceFile, destFile);
            //AddOperation(copyFileProvider);
            var copyFileOperation = new CopyFileOperation(sourceFile, destFile);
            AddOperation(copyFileOperation);
            return this;
        }

        public IOfferRemoteDeployment IisWebApplication(string sourceDir, string webAppName, string webSiteName)
        {
            var webAppProvider = new WebAppDeploymentProvider(sourceDir, webAppName, webSiteName);
            AddOperation(webAppProvider);
            return this;
        }

        public IOfferRemoteDeployment WindowsService(string serviceName, string displayName, string sourceDir, string destDir, string relativeExePath)
        {
            return WindowsService(serviceName, displayName, sourceDir, destDir, relativeExePath, null);
        }

        public IOfferRemoteDeployment WindowsService(string serviceName, string displayName, string sourceDir, string destDir, string relativeExePath, Action<IOfferWindowsServiceOptions> options)
        {
            var winServiceOptions = new WindowsServiceOptions();
            if (options != null)
            {
                options(winServiceOptions);
            }

            var winServiceOperation = new WindowsServiceOperation(serviceName, displayName, sourceDir, destDir, relativeExePath, winServiceOptions.Values);
            AddOperation(winServiceOperation);
            return this;
        }

        public IOfferRemoteDeployment WindowsServiceWithInstaller(string serviceName, string displayName, string sourceDir, string destDir, string relativeExePath, string installerParams)
        {
            return WindowsServiceWithInstaller(serviceName, displayName, sourceDir, destDir, relativeExePath, installerParams, null);
        }

        public IOfferRemoteDeployment WindowsServiceWithInstaller(string serviceName, string displayName, string sourceDir, string destDir, string relativeExePath, string installerParams, Action<IOfferWindowsServiceOptions> options)
        {
            var winServiceOptions = new WindowsServiceOptions();
            if (options != null)
            {
                options(winServiceOptions);
            }

            var winServiceOperation = new WindowsServiceWithInstallerOperation(serviceName, displayName, sourceDir, destDir, relativeExePath, installerParams, winServiceOptions.Values);
            AddOperation(winServiceOperation);
            return this;
        }

        public IOfferRemoteDeployment NServiceBusEndpoint(string sourceDir, string destDir, string serviceName, string profile)
        {
            return NServiceBusEndpoint(sourceDir, destDir, serviceName, profile, null);
        }

        public IOfferRemoteDeployment NServiceBusEndpoint(string sourceDir, string destDir, string serviceName, string profile, Action<IOfferWindowsServiceOptions> options)
        {
            var nServiceBusProvider = new NServiceBusOperation(sourceDir, destDir, serviceName, profile, options);
            AddOperation(nServiceBusProvider);
            return this;
        }

        public IOfferRemoteCertDeployment SslCertificate { get { return new RemoteCertDeploymentBuilder(_remoteSequence, _webDeploy, this); } }

        public void AddOperation(IOperateRemote operation)
        {
            _remoteSequence.Add(operation);    
        }

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
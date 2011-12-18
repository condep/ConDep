using Microsoft.Web.Deployment;

namespace ConDep.Dsl.FluentWebDeploy.Deployment
{
    public class WebDeployOptions
    {
        private readonly DeploymentBaseOptions _sourceBaseOptions;
        private readonly DeploymentBaseOptions _destBaseOptions;
        private readonly DeploymentSyncOptions _syncOptions;

        public WebDeployOptions(DeploymentBaseOptions sourceBaseOptions, DeploymentBaseOptions destBaseOptions, DeploymentSyncOptions syncOptions)
        {
            _sourceBaseOptions = sourceBaseOptions;
            _destBaseOptions = destBaseOptions;
            _syncOptions = syncOptions;
        }

        public DeploymentSyncOptions SyncOptions
        {
            get { return _syncOptions; }
        }

        public DeploymentBaseOptions DestBaseOptions
        {
            get { return _destBaseOptions; }
        }

        public DeploymentBaseOptions SourceBaseOptions
        {
            get { return _sourceBaseOptions; }
        }
    }
}
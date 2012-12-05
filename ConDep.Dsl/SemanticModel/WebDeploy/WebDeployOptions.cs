using Microsoft.Web.Deployment;

namespace ConDep.Dsl.SemanticModel.WebDeploy
{
    public class WebDeployOptions
    {
        private readonly string _packagePath;
        private readonly DeploymentBaseOptions _sourceBaseOptions;
        private readonly DeploymentBaseOptions _destBaseOptions;
        private readonly DeploymentSyncOptions _syncOptions;

        public WebDeployOptions(string packagePath, DeploymentBaseOptions sourceBaseOptions, DeploymentBaseOptions destBaseOptions, DeploymentSyncOptions syncOptions)
        {
            _packagePath = packagePath;
            _sourceBaseOptions = sourceBaseOptions;
            _destBaseOptions = destBaseOptions;
            _syncOptions = syncOptions;
        }

        public string PackagePath
        {
            get { return _packagePath; }
        }

        public bool FromPackage
        {
            get { return !string.IsNullOrWhiteSpace(_packagePath); }
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
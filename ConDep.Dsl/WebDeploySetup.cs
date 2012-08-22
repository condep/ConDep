using System.Collections.Generic;

namespace ConDep.Dsl.Core
{
    public class WebDeploySetup : ISetupWebDeploy
    {
        private readonly ConDepEnvironmentSettings _envSettings;
        private readonly Dictionary<DeploymentServer, WebDeployServerDefinition>  _webDeployDefs = new Dictionary<DeploymentServer, WebDeployServerDefinition>();

        public WebDeploySetup(ConDepEnvironmentSettings envSettings)
        {
            _envSettings = envSettings;
        }

        public ConDepEnvironmentSettings EnvSettings
        {
            get { return _envSettings; }
        }

        public void ConfigureServer(DeploymentServer deploymentServer, ConDepSetup setup)
        {
            if (_webDeployDefs.ContainsKey(deploymentServer))
                return;

            ActiveDeploymentServer = deploymentServer;

            var webDeployServerDefinition = WebDeployServerDefinition.CreateOrGetForServer(EnvSettings, deploymentServer);
            _webDeployDefs.Add(deploymentServer, webDeployServerDefinition);

            //Todo: Check if this should be done before or after calling the action
            var webDeployOperation = new WebDeployOperation(webDeployServerDefinition);
            setup.AddOperation(webDeployOperation);
        }

        public void ConfigureProvider(IProvide provider)
        {
            ActiveWebDeployServerDefinition.Providers.Add(provider);
        }

        public void ConfigureProvider(WebDeployCompositeProviderBase provider)
        {
            provider.Configure(ActiveDeploymentServer);
            ActiveWebDeployServerDefinition.Providers.Add(provider);
        }

        public DeploymentServer ActiveDeploymentServer { get; private set; }
        
        public WebDeployServerDefinition ActiveWebDeployServerDefinition
        {
            get
            {
                return _webDeployDefs[ActiveDeploymentServer];
            }
        }
    }
}
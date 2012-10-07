using ConDep.Dsl.Model.Config;

namespace ConDep.Dsl.WebDeploy
{
    internal class WebDeploySetup : ISetupWebDeploy
    {
        private readonly ConDepConfig _envConfig;
        private ServerConfig _activeDeploymentServer;

        public WebDeploySetup(ConDepConfig envConfig)
        {
            _envConfig = envConfig;
        }

        public WebDeployServerDefinition ConfigureServer(ServerConfig deploymentServer)
        {
            _activeDeploymentServer = deploymentServer;
            ActiveWebDeployServerDefinition = WebDeployServerDefinition.CreateOrGetForServer(deploymentServer);
            return ActiveWebDeployServerDefinition;
        }

        public void ConfigureProvider(IProvide provider)
        {
            var webDeployCompositeProviderBase = provider as WebDeployCompositeProviderBase;
            if (webDeployCompositeProviderBase != null)
            {
                webDeployCompositeProviderBase.Configure(_activeDeploymentServer);
            }
            ActiveWebDeployServerDefinition.AddProvider(provider, _envConfig);
        }

        public WebDeployServerDefinition ActiveWebDeployServerDefinition { get; private set; }
    }
}
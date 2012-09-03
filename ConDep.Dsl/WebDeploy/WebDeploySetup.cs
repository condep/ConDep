namespace ConDep.Dsl.WebDeploy
{
    public class WebDeploySetup : ISetupWebDeploy
    {
        private DeploymentServer _activeDeploymentServer;

        public WebDeployServerDefinition ConfigureServer(DeploymentServer deploymentServer)
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
            ActiveWebDeployServerDefinition.Providers.Add(provider);
        }

        public WebDeployServerDefinition ActiveWebDeployServerDefinition { get; private set; }
    }
}
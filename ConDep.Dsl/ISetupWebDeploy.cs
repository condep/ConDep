namespace ConDep.Dsl.Core
{
    public interface ISetupWebDeploy
    {
        ConDepEnvironmentSettings EnvSettings { get; }
        DeploymentServer ActiveDeploymentServer { get; }
        WebDeployServerDefinition ActiveWebDeployServerDefinition { get; }
        void ConfigureServer(DeploymentServer deploymentServer, ConDepSetup setup);
        void ConfigureProvider(IProvide provider);
        void ConfigureProvider(WebDeployCompositeProviderBase provider);
    }
}
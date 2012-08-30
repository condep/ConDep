namespace ConDep.Dsl
{
    public interface ISetupWebDeploy
    {
        //DeploymentServer ActiveDeploymentServer { get; set; }
        //WebDeployServerDefinition ActiveWebDeployServerDefinition { get; }
        WebDeployServerDefinition ConfigureServer(DeploymentServer deploymentServer);
        void ConfigureProvider(IProvide provider);
        void ConfigureProvider(WebDeployCompositeProviderBase provider);
        WebDeployServerDefinition ActiveWebDeployServerDefinition { get; }
    }
}
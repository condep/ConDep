using ConDep.Dsl.WebDeploy;

namespace ConDep.Dsl
{
    public interface ISetupWebDeploy
    {
        WebDeployServerDefinition ConfigureServer(DeploymentServer deploymentServer);
        void ConfigureProvider(IProvide provider);
        WebDeployServerDefinition ActiveWebDeployServerDefinition { get; }
    }
}
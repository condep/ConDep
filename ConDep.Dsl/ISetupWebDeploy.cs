using ConDep.Dsl.Model.Config;
using ConDep.Dsl.WebDeploy;

namespace ConDep.Dsl
{
    public interface ISetupWebDeploy
    {
        WebDeployServerDefinition ConfigureServer(ServerConfig deploymentServer);
        void ConfigureProvider(IProvide provider);
        WebDeployServerDefinition ActiveWebDeployServerDefinition { get; }
    }
}
using ConDep.Dsl.Config;

namespace ConDep.Dsl.WebDeploy
{
    public interface IProvideConditions
    {
        void Configure(ServerConfig server);
        bool HasExpectedOutcome(WebDeployOptions webDeployOptions);
    }
}
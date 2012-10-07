using ConDep.Dsl.Model.Config;
using ConDep.Dsl.WebDeploy;

namespace ConDep.Dsl
{
    public interface IProvideConditions
    {
        void Configure(ServerConfig server);
        bool HasExpectedOutcome(WebDeployOptions webDeployOptions);
    }
}
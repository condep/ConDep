using ConDep.Dsl.WebDeploy;

namespace ConDep.Dsl
{
    public interface IProvideConditions
    {
        void Configure(DeploymentServer arrServer);
        bool HasExpectedOutcome(WebDeployOptions webDeployOptions);
    }
}
namespace ConDep.Dsl.Core
{
    public interface IProvideForDeployment : IProvideForAll
    {
        DeploymentIisOptions IIS { get; }
    }
}
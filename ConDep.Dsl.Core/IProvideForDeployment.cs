namespace ConDep.Dsl.Core
{
    public interface IProvideForDeployment : IProvideForAll
    {
        IisDeploymentOptions IIS { get; }
    }
}
namespace ConDep.Dsl.Core
{
    public interface IProvideForDeployment : IProviderForAll
    {
        IisDeploymentOptions IIS { get; }
    }
}
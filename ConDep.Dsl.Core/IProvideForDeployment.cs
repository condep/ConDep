namespace ConDep.Dsl.Core
{
    public interface IProvideForDeployment : IProviderCollection
    {
        IisDeploymentOptions IIS { get; }
    }
}
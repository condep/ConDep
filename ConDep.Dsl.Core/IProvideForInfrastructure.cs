namespace ConDep.Dsl.Core
{
    public interface IProvideForInfrastructure : IProviderCollection
    {
        IisDeploymentOptions IIS { get; }
        WindowsOptions Windows { get; }
    }
}
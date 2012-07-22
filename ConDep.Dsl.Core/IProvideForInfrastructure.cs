namespace ConDep.Dsl.Core
{
    public interface IProvideForInfrastructure : IProviderCollection
    {
        InfrastructureIisOptions IIS { get; }
        InfrastructureWindowsOptions Windows { get; }
    }
}
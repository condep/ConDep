namespace ConDep.Dsl.Core
{
    public interface IProvideForInfrastructure : IProviderForAll
    {
        InfrastructureIisOptions IIS { get; }
        InfrastructureWindowsOptions Windows { get; }
    }
}
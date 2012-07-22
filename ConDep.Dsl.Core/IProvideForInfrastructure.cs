namespace ConDep.Dsl.Core
{
    public interface IProvideForInfrastructure : IProvideForAll
    {
        InfrastructureIisOptions IIS { get; }
        InfrastructureWindowsOptions Windows { get; }
    }
}
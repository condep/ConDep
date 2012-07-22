namespace ConDep.Dsl.Core
{
    public interface IProvideForInfrastrucutreWebSite : IProviderForAll
    {
        string WebSiteName { get; }
        string AppPoolName { get; set; }
    }
}
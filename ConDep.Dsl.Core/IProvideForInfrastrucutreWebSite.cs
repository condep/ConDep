namespace ConDep.Dsl.Core
{
    public interface IProvideForInfrastrucutreWebSite : IProviderCollection
    {
        string WebSiteName { get; }
        string AppPoolName { get; set; }
    }
}
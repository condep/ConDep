namespace ConDep.Dsl.Core
{
    public interface IProvideForInfrastrucutreWebSite : IProvideForAll
    {
        string WebSiteName { get; }
        string AppPoolName { get; set; }
    }
}
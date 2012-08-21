namespace ConDep.Dsl.Core
{
    public interface IProvideForInfrastrucutreWebSite
    {
        string WebSiteName { get; }
        string AppPoolName { get; set; }
    }
}
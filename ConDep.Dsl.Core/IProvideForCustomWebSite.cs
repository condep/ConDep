namespace ConDep.Dsl.Core
{
    public interface IProvideForCustomWebSite : IProviderCollection
    {
        string WebSiteName { get; }
    }
}
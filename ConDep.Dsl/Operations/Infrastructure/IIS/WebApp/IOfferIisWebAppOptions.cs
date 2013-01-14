namespace ConDep.Dsl.Builders
{
    public interface IOfferIisWebAppOptions
    {
        IOfferIisWebAppOptions PhysicalPath(string path);
        IOfferIisWebAppOptions AppPool(string name);
    }
}
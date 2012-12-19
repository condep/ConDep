namespace ConDep.Dsl.Operations.Infrastructure.IIS.WebSite
{
    public interface IOfferIisWebSiteOptions
    {
        IOfferIisWebSiteOptions Port(int port);
    }

    public class IisWebSiteOptions : IOfferIisWebSiteOptions
    {
        public IOfferIisWebSiteOptions Port(int port)
        {
            PortNumber = port;
            return this;
        }

        public int PortNumber { get; set; }
    }
}
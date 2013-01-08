namespace ConDep.Dsl.Operations.Infrastructure.IIS.WebSite
{
    public interface IOfferBindingOptions
    {
        IOfferBindingOptions Ip(string ip);
        IOfferBindingOptions Port(int port);
        IOfferBindingOptions HostName(string hostName);
    }
}
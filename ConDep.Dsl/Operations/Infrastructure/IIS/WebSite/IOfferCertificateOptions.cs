namespace ConDep.Dsl.Operations.Infrastructure.IIS.WebSite
{
    public interface IOfferCertificateOptions
    {
        IOfferCertificateOptions AddPrivateKeyPermission(string user);
    }
}
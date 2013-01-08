namespace ConDep.Dsl.Operations.Infrastructure.IIS.WebSite
{
    public interface IOfferCertificateBindingOptions
    {
        IOfferCertificateBindingOptions FromCertStore(string commonName);
        IOfferCertificateBindingOptions FromFile(string filePath);
    }
}
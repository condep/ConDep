namespace ConDep.Dsl
{
    public interface IOfferCertificateOptions
    {
        /// <summary>
        /// Give users read access to the private key for a certificate
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        IOfferCertificateOptions AddPrivateKeyPermission(string user);
    }
}
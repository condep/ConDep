using System;
using System.Security.Cryptography.X509Certificates;

namespace ConDep.Dsl.Operations.Infrastructure.IIS.WebSite
{
    public interface IOfferIisWebSiteOptions
    {
        IOfferIisWebSiteOptions PhysicalPath(string path);

        IOfferIisWebSiteOptions AddHttpBinding(Action<IOfferBindingOptions> httpBindingOptions);
        /// <summary>
        /// Finds certifcate in local certificate store and copies certificate to server and adds to binding.
        /// </summary>
        /// <param name="findType">Type of certificate attribute to search in.</param>
        /// <param name="findName">Value of certificate attribute.</param>
        /// <param name="bindingOptions">Binding options for setting port, ip and host name.</param>
        /// <returns></returns>
        IOfferIisWebSiteOptions AddHttpsBinding(X509FindType findType, string findName, Action<IOfferBindingOptions> bindingOptions);

        /// <summary>
        /// Copies certificate without private key from local file to server, add to cert store on server and adds to binding.
        /// </summary>
        /// <param name="filePath">File path to certificate (.cer)</param>
        /// <param name="bindingOptions">Binding options for setting port, ip and host name.</param>
        /// <returns></returns>
        IOfferIisWebSiteOptions AddHttpsBinding(string filePath, Action<IOfferBindingOptions> bindingOptions);

        /// <summary>
        /// Copies certificate with private key from local file to server, add to cert store on server and adds to binding.
        /// </summary>
        /// <param name="filePath">File path to certificate with private key (.pfx)</param>
        /// <param name="privateKeyPassword">Password to use for the private key. This password is needed to read and install private key server side.</param>
        /// <param name="bindingOptions">Binding options for setting port, ip and host name.</param>
        /// <returns></returns>
        IOfferIisWebSiteOptions AddHttpsBinding(string filePath, string privateKeyPassword, Action<IOfferBindingOptions> bindingOptions);
        IOfferIisWebSiteOptions ApplicationPool(string appPoolName);
    }
}
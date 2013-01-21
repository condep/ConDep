using System;
using System.Security.Cryptography.X509Certificates;
using ConDep.Dsl.Builders;

namespace ConDep.Dsl.Operations.Infrastructure.IIS.WebSite
{
    public interface IOfferIisWebSiteOptions
    {
        /// <summary>
        /// Sets the physical path of the Web Site.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        IOfferIisWebSiteOptions PhysicalPath(string path);

        /// <summary>
        /// Adds a Http Binding to the Web Site.
        /// </summary>
        /// <param name="httpBindingOptions">At least one of the options must be specified for the binding to be valid.</param>
        /// <returns></returns>
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
        /// Associate application pool with Web Site. To create an Application Pool, use the IISAppPool operation (e.g. require.IISAppPool)
        /// </summary>
        /// <param name="appPoolName"></param>
        /// <returns></returns>
        IOfferIisWebSiteOptions ApplicationPool(string appPoolName);

        /// <summary>
        /// Adds an empty Web Application to the Web Site
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        IOfferIisWebSiteOptions WebApp(string name);

        /// <summary>
        /// Adds an empty Web Application with defined options to the Web Site.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        IOfferIisWebSiteOptions WebApp(string name, Action<IOfferIisWebAppOptions> options);
    }
}
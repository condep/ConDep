using System;
using ConDep.Dsl.Config;
using ConDep.Dsl.Operations.Infrastructure;

namespace ConDep.Dsl
{
    public interface IOfferInfrastructure
    {
        /// <summary>
        /// Installs and configures IIS with provided options
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        IOfferInfrastructure IIS(Action<IisInfrastructureOptions> options);

        /// <summary>
        /// Installs IIS
        /// </summary>
        /// <returns></returns>
        IOfferInfrastructure IIS();

        /// <summary>
        /// Offer common Windows operations
        /// </summary>
        /// <returns></returns>
        IOfferInfrastructure Windows(Action<WindowsInfrastructureOptions> options);

        /// <summary>
        /// Creates a new Web Site in IIS if not exist. If exist, will delete and then create new.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        IOfferInfrastructure IISWebSite(string name, int id);

        /// <summary>
        /// Creates a new Web Site in IIS if not exist. If exist, will delete and then create new with provided options.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="id"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        IOfferInfrastructure IISWebSite(string name, int id, Action<IOfferIisWebSiteOptions> options);

        /// <summary>
        /// Will create a new Application Pool in IIS.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        IOfferInfrastructure IISAppPool(string name);

        /// <summary>
        /// Will create a new Application Pool in IIS with provided options.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        IOfferInfrastructure IISAppPool(string name, Action<IOfferIisAppPoolOptions> options);

        /// <summary>
        /// Will create a new Web Application in IIS under the given Web Site.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="webSite"></param>
        /// <returns></returns>
        IOfferInfrastructure IISWebApp(string name, string webSite);

        /// <summary>
        /// Will create a new Web Application in IIS under the given Web Site, with the provided options.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="webSite"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        IOfferInfrastructure IISWebApp(string name, string webSite, Action<IOfferIisWebAppOptions> options);

        /// <summary>
        /// Provide operations for installing SSL certificates.
        /// </summary>
        IOfferSslInfrastructure SslCertificate { get; }

        /// <summary>
        /// Provide operations for remote execution.
        /// </summary>
        /// <param name="remoteExecution"></param>
        /// <returns></returns>
        IOfferInfrastructure RemoteExecution(Action<IOfferRemoteExecution> remoteExecution);

        /// <summary>
        /// Provide operations for remote deployment.
        /// </summary>
        /// <param name="remoteDeployment"></param>
        /// <returns></returns>
        IOfferInfrastructure RemoteDeployment(Action<IOfferRemoteDeployment> remoteDeployment);

        /// <summary>
        /// Server side condition. Any Operation followed by <see cref="OnlyIf"/> will only execute if the condition is met.
        /// </summary>
        /// <param name="condition">The condition that must be met</param>
        /// <returns></returns>
        IOfferInfrastructure OnlyIf(Predicate<ServerInfo> condition);
    }
}
using System;
using ConDep.Dsl.Operations.Infrastructure;
using ConDep.Dsl.Operations.Infrastructure.IIS;
using ConDep.Dsl.Operations.Infrastructure.IIS.WebSite;

namespace ConDep.Dsl.Builders
{
    public interface IOfferInfrastructure
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="unknown"> </param>
        /// <returns></returns>
        IOfferInfrastructure IIS(Action<IisInfrastructureOptions> options);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IOfferInfrastructure IIS();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IOfferInfrastructure MSMQ();
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        IOfferInfrastructure IISWebSite(string name, int id);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="id"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        IOfferInfrastructure IISWebSite(string name, int id, Action<IOfferIisWebSiteOptions> options);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        IOfferInfrastructure IISAppPool(string name);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        IOfferInfrastructure IISAppPool(string name, Action<IisAppPoolOptions> options);
    }
}
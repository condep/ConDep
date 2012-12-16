using System;
using ConDep.Dsl.Operations.Infrastructure;

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
        /// <returns></returns>
        IOfferIisAppPool IISAppPool(string name);
    }
}
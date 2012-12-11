using System;
using ConDep.Dsl.Operations.Infrastructure;

namespace ConDep.Dsl.Builders
{
    public class InfrastructureBuilder : IOfferInfrastructure
    {
        private readonly IOfferRemoteOperations _remote;
        //private readonly IisInfrastructureOperation _operation;

        //public InfrastructureBuilder(IisInfrastructureOperation operation)
        //{
        //    _operation = operation;
        //}

        public InfrastructureBuilder(IOfferRemoteOperations remote)
        {
            _remote = remote;
        }

        public IOfferInfrastructure IIS(Action<IisInfrastructureOptions> options)
        {
            var iisOperation = new IisInfrastructureOperation();
            options(new IisInfrastructureOptions(iisOperation));
            iisOperation.Configure(_remote);
            return this;
        }

        public IOfferInfrastructure MSMQ()
        {
            throw new NotImplementedException();
        }

        public IOfferIisWebSite IISWebSite(string name, int id)
        {
            throw new NotImplementedException();
        }

        public IOfferIisAppPool IISAppPool(string name)
        {
            throw new NotImplementedException();
        }
    }
}
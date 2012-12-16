using System;
using ConDep.Dsl.Operations.Infrastructure;
using ConDep.Dsl.SemanticModel.Sequence;
using ConDep.Dsl.SemanticModel.WebDeploy;

namespace ConDep.Dsl.Builders
{
    public class InfrastructureBuilder : IOfferInfrastructure
    {
        private readonly IManageInfrastructureSequence _infrastructureSequence;
        private readonly IOperateWebDeploy _webDeploy;
        //private readonly IOfferRemoteOperations _remote;

        //public InfrastructureBuilder(IOfferRemoteOperations remote)
        //{
        //    _remote = remote;
        //}

        public InfrastructureBuilder(IManageInfrastructureSequence infrastructureSequence, IOperateWebDeploy webDeploy)
        {
            _infrastructureSequence = infrastructureSequence;
            _webDeploy = webDeploy;
        }

        public IOfferInfrastructure IIS(Action<IisInfrastructureOptions> options)
        {
            var iisOperation = new IisInfrastructureOperation();
            options(new IisInfrastructureOptions(iisOperation));
            iisOperation.Configure(new RemoteCompositeBuilder(_infrastructureSequence.NewCompositeSequence("IIS"), _webDeploy));
            //_infrastructureSequence.Add(iisOperation);
            //iisOperation.Configure(_remote);
            return this;
        }

        public IOfferInfrastructure IIS()
        {
            var iisOperation = new IisInfrastructureOperation();
            iisOperation.Configure(new RemoteCompositeBuilder(_infrastructureSequence.NewCompositeSequence("IIS"), _webDeploy));
            //_infrastructureSequence.Add(iisOperation);
            //iisOperation.Configure(_remote);
            return this;
        }

        public IOfferInfrastructure MSMQ()
        {
            throw new NotImplementedException();
        }

        public IOfferInfrastructure IISWebSite(string name, int id)
        {
            throw new NotImplementedException();
        }

        public IOfferIisAppPool IISAppPool(string name)
        {
            throw new NotImplementedException();
        }
    }
}
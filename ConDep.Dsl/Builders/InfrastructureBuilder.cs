using System;
using ConDep.Dsl.Operations.Infrastructure;
using ConDep.Dsl.Operations.Infrastructure.IIS;
using ConDep.Dsl.Operations.Infrastructure.IIS.AppPool;
using ConDep.Dsl.Operations.Infrastructure.IIS.WebSite;
using ConDep.Dsl.SemanticModel.Sequence;
using ConDep.Dsl.SemanticModel.WebDeploy;

namespace ConDep.Dsl.Builders
{
    public class InfrastructureBuilder : IOfferInfrastructure
    {
        private readonly IManageInfrastructureSequence _infrastructureSequence;
        private readonly IHandleWebDeploy _webDeploy;
        //private readonly IOfferRemoteOperations _remote;

        //public InfrastructureBuilder(IOfferRemoteOperations remote)
        //{
        //    _remote = remote;
        //}

        public InfrastructureBuilder(IManageInfrastructureSequence infrastructureSequence, IHandleWebDeploy webDeploy)
        {
            _infrastructureSequence = infrastructureSequence;
            _webDeploy = webDeploy;
        }

        public IOfferInfrastructure IIS(Action<IisInfrastructureOptions> options)
        {
            var iisOperation = new IisInfrastructureOperation();
            options(new IisInfrastructureOptions(iisOperation));
            iisOperation.Configure(new RemoteCompositeBuilder(_infrastructureSequence.NewCompositeSequence(iisOperation), _webDeploy));
            return this;
        }

        public IOfferInfrastructure IIS()
        {
            var iisOperation = new IisInfrastructureOperation();
            iisOperation.Configure(new RemoteCompositeBuilder(_infrastructureSequence.NewCompositeSequence(iisOperation), _webDeploy));
            return this;
        }

        public IOfferInfrastructure MSMQ()
        {
            throw new NotImplementedException();
        }

        public IOfferInfrastructure IISWebSite(string name, int id)
        {
            var webSiteOperation = new IisWebSiteOperation(name, id);
            webSiteOperation.Configure(new RemoteCompositeBuilder(_infrastructureSequence.NewCompositeSequence(webSiteOperation), _webDeploy));
            return this;
        }

        public IOfferInfrastructure IISWebSite(string name, int id, Action<IOfferIisWebSiteOptions> options)
        {
            var webSiteOptions = new IisWebSiteOptions();
            options(webSiteOptions);
            var webSiteOperation = new IisWebSiteOperation(name, id, webSiteOptions);
            webSiteOperation.Configure(new RemoteCompositeBuilder(_infrastructureSequence.NewCompositeSequence(webSiteOperation), _webDeploy));
            return this;
        }

        public IOfferInfrastructure IISAppPool(string name)
        {
            var op = new IisAppPoolInfrastructureOperation(name);
            op.Configure(new RemoteCompositeBuilder(_infrastructureSequence.NewCompositeSequence(op), _webDeploy));
            return this;
        }

        public IOfferInfrastructure IISAppPool(string name, Action<IisAppPoolOptions> options)
        {
            var appPoolOptions = new IisAppPoolOptions();
            options(appPoolOptions);

            var appPoolOperation = new IisAppPoolInfrastructureOperation(name, appPoolOptions);

            appPoolOperation.Configure(new RemoteCompositeBuilder(_infrastructureSequence.NewCompositeSequence(appPoolOperation), _webDeploy));
            return this;
        }
    }
}
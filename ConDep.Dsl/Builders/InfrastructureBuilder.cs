using System;
using ConDep.Dsl.Operations;
using ConDep.Dsl.Operations.Infrastructure;
using ConDep.Dsl.Operations.Infrastructure.IIS;
using ConDep.Dsl.Operations.Infrastructure.IIS.AppPool;
using ConDep.Dsl.Operations.Infrastructure.IIS.WebApp;
using ConDep.Dsl.Operations.Infrastructure.IIS.WebSite;
using ConDep.Dsl.Operations.Windows;
using ConDep.Dsl.SemanticModel;

namespace ConDep.Dsl.Builders
{
    public class InfrastructureBuilder : IOfferInfrastructure, IConfigureInfrastructure
    {
        private readonly IManageInfrastructureSequence _infrastructureSequence;

        public InfrastructureBuilder(IManageInfrastructureSequence infrastructureSequence)
        {
            _infrastructureSequence = infrastructureSequence;
        }

        public IOfferInfrastructure IIS(Action<IisInfrastructureOptions> options)
        {
            var iisOperation = new IisInfrastructureOperation();
            options(new IisInfrastructureOptions(iisOperation));
            AddOperation(iisOperation);
            return this;
        }

        public IOfferInfrastructure IIS()
        {
            var iisOperation = new IisInfrastructureOperation();
            AddOperation(iisOperation);
            return this;
        }

        public IOfferInfrastructure Windows(Action<WindowsInfrastructureOptions> options)
        {
            var windowsOperation = new WindowsFeatureInfrastructureOperation();
            options(new WindowsInfrastructureOptions(windowsOperation));
            AddOperation(windowsOperation);
            return this;
        }

        public IOfferInfrastructure IISWebSite(string name, int id)
        {
            var webSiteOperation = new IisWebSiteOperation(name, id);
            AddOperation(webSiteOperation);
            return this;
        }

        public IOfferInfrastructure IISWebSite(string name, int id, Action<IOfferIisWebSiteOptions> options)
        {
            var webSiteOptions = new IisWebSiteOptions();
            options(webSiteOptions);
            var webSiteOperation = new IisWebSiteOperation(name, id, webSiteOptions);
            AddOperation(webSiteOperation);
            return this;
        }

        public IOfferInfrastructure IISAppPool(string name)
        {
            var op = new IisAppPoolOperation(name);
            AddOperation(op);
            return this;
        }

        public IOfferInfrastructure IISAppPool(string name, Action<IOfferIisAppPoolOptions> options)
        {
            var appPoolOptions = new IisAppPoolOptions();
            options(appPoolOptions);

            var appPoolOperation = new IisAppPoolOperation(name, appPoolOptions.Values);

            AddOperation(appPoolOperation);
            return this;
        }

        public IOfferInfrastructure IISWebApp(string name, string webSite)
        {
            var op = new IisWebAppOperation(name, webSite);
            AddOperation(op);
            return this;
        }

        public IOfferInfrastructure IISWebApp(string name, string webSite, Action<IOfferIisWebAppOptions> options)
        {
            var webAppOptions = new IisWebAppOptions(name);
            options(webAppOptions);

            var op = new IisWebAppOperation(name, webSite, webAppOptions.Values);
            AddOperation(op);
            return this;
        }

        public IOfferSslInfrastructure SslCertificate
        {
            get { return new SslInfrastructureBuilder(_infrastructureSequence, this); }
        }

        public IOfferInfrastructure RemoteExecution(Action<IOfferRemoteExecution> remoteExecution)
        {
            remoteExecution(new RemoteExecutionBuilder(_infrastructureSequence));
            return this;
        }

        //public IOfferRemoteExecution RemoteExecution { get{ return new RemoteExecutionBuilder(_infrastructureSequence, _webDeploy);} }

        public void AddOperation(RemoteCompositeInfrastructureOperation operation)
        {
            operation.Configure(new RemoteCompositeBuilder(_infrastructureSequence.NewCompositeSequence(operation)), new InfrastructureBuilder(_infrastructureSequence));
        }
    }
}
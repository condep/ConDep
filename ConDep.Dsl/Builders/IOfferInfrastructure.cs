using System;
using System.Security.Cryptography.X509Certificates;
using ConDep.Dsl.Operations.Application.Deployment.Certificate;
using ConDep.Dsl.Operations.Infrastructure;
using ConDep.Dsl.Operations.Infrastructure.IIS;
using ConDep.Dsl.Operations.Infrastructure.IIS.WebSite;
using ConDep.Dsl.SemanticModel.Sequence;
using ConDep.Dsl.SemanticModel.WebDeploy;

namespace ConDep.Dsl.Builders
{
    public interface IOfferInfrastructure
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
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
        IOfferInfrastructure IISAppPool(string name, Action<IOfferIisAppPoolOptions> options);

        IOfferInfrastructure IISWebApp(string name, string webSite);
        
        IOfferInfrastructure IISWebApp(string name, string webSite, Action<IOfferIisWebAppOptions> options);

        IOfferSslInfrastructure SslCertificate { get; }
    }

    public interface IOfferSslInfrastructure
    {
        IOfferInfrastructure FromStore(X509FindType findType, string findValue, Action<IOfferCertificateOptions> options = null);
        IOfferInfrastructure FromFile(string path, string password, Action<IOfferCertificateOptions> options = null);
    }

    public class SslInfrastructureBuilder : IOfferSslInfrastructure
    {
        private readonly IManageInfrastructureSequence _infrastructureSequence;
        private readonly IHandleWebDeploy _webDeploy;
        private readonly InfrastructureBuilder _infrastructureBuilder;

        public SslInfrastructureBuilder(IManageInfrastructureSequence infrastructureSequence, IHandleWebDeploy webDeploy, InfrastructureBuilder infrastructureBuilder)
        {
            _infrastructureSequence = infrastructureSequence;
            _webDeploy = webDeploy;
            _infrastructureBuilder = infrastructureBuilder;
        }

        public IOfferInfrastructure FromStore(X509FindType findType, string findValue)
        {
            var certOp = new CertificateFromStoreOperation(findType, findValue);
            var compositeSequence = _infrastructureSequence.NewCompositeSequence(certOp);
            certOp.Configure(new RemoteCompositeBuilder(compositeSequence, _webDeploy));
            return _infrastructureBuilder;
        }

        public IOfferInfrastructure FromStore(X509FindType findType, string findValue, Action<IOfferCertificateOptions> options)
        {
            var certOpt = new CertificateOptions();
            options(certOpt);

            var certOp = new CertificateFromStoreOperation(findType, findValue, certOpt);
            var compositeSequence = _infrastructureSequence.NewCompositeSequence(certOp);
            certOp.Configure(new RemoteCompositeBuilder(compositeSequence, _webDeploy));
            return _infrastructureBuilder;
        }

        public IOfferInfrastructure FromFile(string path, string password)
        {
            var certOp = new CertificateFromFileOperation(path, password);
            var compositeSequence = _infrastructureSequence.NewCompositeSequence(certOp);
            certOp.Configure(new RemoteCompositeBuilder(compositeSequence, _webDeploy));
            return _infrastructureBuilder;
        }

        public IOfferInfrastructure FromFile(string path, string password, Action<IOfferCertificateOptions> options)
        {
            var certOpt = new CertificateOptions();
            options(certOpt);

            var certOp = new CertificateFromFileOperation(path, password, certOpt);
            var compositeSequence = _infrastructureSequence.NewCompositeSequence(certOp);
            certOp.Configure(new RemoteCompositeBuilder(compositeSequence, _webDeploy));
            return _infrastructureBuilder;
        }
    }
}
using System;
using System.Security.Cryptography.X509Certificates;
using ConDep.Dsl.Operations.Application.Deployment.Certificate;
using ConDep.Dsl.Operations.Infrastructure.IIS.WebSite;
using ConDep.Dsl.SemanticModel;
using ConDep.Dsl.SemanticModel.Sequence;
using ConDep.Dsl.SemanticModel.WebDeploy;

namespace ConDep.Dsl.Builders
{
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
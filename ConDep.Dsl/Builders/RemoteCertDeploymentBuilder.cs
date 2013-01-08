using System.Security.Cryptography.X509Certificates;
using ConDep.Dsl.Operations.Application.Deployment.Certificate;
using ConDep.Dsl.SemanticModel;
using ConDep.Dsl.SemanticModel.Sequence;
using ConDep.Dsl.SemanticModel.WebDeploy;

namespace ConDep.Dsl.Builders
{
    public class RemoteCertDeploymentBuilder : IOfferRemoteCertDeployment
    {
        private readonly IManageRemoteSequence _remoteSequence;
        private readonly IHandleWebDeploy _webDeploy;
        private readonly IOfferRemoteDeployment _remoteDeploymentBuilder;

        public RemoteCertDeploymentBuilder(IManageRemoteSequence remoteSequence, IHandleWebDeploy webDeploy, IOfferRemoteDeployment remoteDeploymentBuilder)
        {
            _remoteSequence = remoteSequence;
            _webDeploy = webDeploy;
            _remoteDeploymentBuilder = remoteDeploymentBuilder;
        }

        public IOfferRemoteDeployment FromStore(string thumbprint)
        {
            var certProvider = new CertficiateDeploymentProvider(thumbprint);
            _remoteSequence.Add(new RemoteWebDeployOperation(certProvider, _webDeploy));
            return _remoteDeploymentBuilder;
        }

        public IOfferRemoteDeployment FromStore(X509FindType findType, string findValue)
        {
            var certOp = new CertificateFromStoreOperation(findType, findValue);
            var compositeSequence = _remoteSequence.NewCompositeSequence(certOp);
            certOp.Configure(new RemoteCompositeBuilder(compositeSequence, _webDeploy));
            return _remoteDeploymentBuilder;
        }

        public IOfferRemoteDeployment FromFile(string path, string password)
        {
            var certOp = new CertificateFromFileOperation(path, password);
            var compositeSequence = _remoteSequence.NewCompositeSequence(certOp);
            certOp.Configure(new RemoteCompositeBuilder(compositeSequence, _webDeploy));
            return _remoteDeploymentBuilder;
        }
    }
}
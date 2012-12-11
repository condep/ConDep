using ConDep.Dsl.Operations.Application.Deployment.Certificate;
using ConDep.Dsl.SemanticModel;
using ConDep.Dsl.SemanticModel.Sequence;
using ConDep.Dsl.SemanticModel.WebDeploy;
using IOperateWebDeploy = ConDep.Dsl.SemanticModel.WebDeploy.IOperateWebDeploy;

namespace ConDep.Dsl.Builders
{
    public class RemoteCertDeploymentBuilder : IOfferRemoteCertDeployment
    {
        private readonly IManageRemoteSequence _remoteSequence;
        private readonly IOperateWebDeploy _webDeploy;
        private readonly IOfferRemoteDeployment _remoteDeploymentBuilder;
        private readonly IOfferRemoteOperations _remoteBuilder;

        public RemoteCertDeploymentBuilder(IManageRemoteSequence remoteSequence, IOperateWebDeploy webDeploy, IOfferRemoteDeployment remoteDeploymentBuilder, IOfferRemoteOperations remoteBuilder)
        {
            _remoteSequence = remoteSequence;
            _webDeploy = webDeploy;
            _remoteDeploymentBuilder = remoteDeploymentBuilder;
            _remoteBuilder = remoteBuilder;
        }

        public IOfferRemoteDeployment FromStore(string thumbprint)
        {
            var certProvider = new CertficiateDeploymentProvider(thumbprint);
            _remoteSequence.Add(new RemoteWebDeployOperation(certProvider, _webDeploy));
            return _remoteDeploymentBuilder;
        }

        public IOfferRemoteDeployment FromFile(string path, string password)
        {
            var certOp = new CertificateOperation(path, password);
            certOp.Configure(_remoteBuilder);
            return _remoteDeploymentBuilder;
        }
    }
}
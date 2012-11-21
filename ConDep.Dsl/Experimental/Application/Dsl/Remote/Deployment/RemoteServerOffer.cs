using System.Collections.Generic;
using ConDep.Dsl.Experimental.Application.Deployment;
using ConDep.Dsl.Experimental.Core;
using ConDep.Dsl.Model.Config;

namespace ConDep.Dsl.Experimental.Application
{
    public class RemoteServerOffer : IOfferRemoteOptions
    {
        private readonly IOperateWebDeploy _webDeploy;
        private readonly ILogForConDep _logger;
        private readonly IManageRemoteSequence _remoteSequence;

        public RemoteServerOffer(IManageExecutionSequence executionSequence, IEnumerable<ServerConfig> servers, IOperateWebDeploy webDeploy, ILogForConDep logger)
        {
            _webDeploy = webDeploy;
            _logger = logger;
            _remoteSequence = executionSequence.NewRemoteSequence(servers);
 
            Deploy = new RemoteDeployment(_remoteSequence, new SslCertificateDeployer(), _webDeploy, _logger);
            ExecuteRemote = new RemoteExecutor();
        }

        public IOfferRemoteDeployment Deploy { get; private set; }
        public IOfferRemoteExecution ExecuteRemote { get; private set; }
        //public IOperateLocally FromLocalMachineToServer
        //{
        //    get {
        //        return new LocalExecutor(_remoteSequence.NewLocalSequence());
        //    }
        //}
    }
}
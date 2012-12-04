using System.Collections.Generic;
using ConDep.Dsl.Config;
using ConDep.Dsl.SemanticModel.Sequence;
using ConDep.Dsl.WebDeploy;

namespace ConDep.Dsl.Builders
{
    public class RemoteOperationsBuilder : IOfferRemoteOperations
    {
        private readonly IOperateWebDeploy _webDeploy;
        private readonly IManageRemoteSequence _remoteSequence;

        public RemoteOperationsBuilder(IManageExecutionSequence executionSequence, IEnumerable<ServerConfig> servers, IOperateWebDeploy webDeploy)
        {
            _webDeploy = webDeploy;
            _remoteSequence = executionSequence.NewRemoteSequence(servers);
 
            Deploy = new RemoteDeploymentBuilder(_remoteSequence, new RemoteCertDeploymentBuilder(), _webDeploy, this);
            ExecuteRemote = new RemoteExecutionBuilder(_remoteSequence, webDeploy, this);
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
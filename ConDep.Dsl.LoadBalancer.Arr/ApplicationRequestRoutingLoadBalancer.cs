using ConDep.Dsl.Builders;
using ConDep.Dsl.Config;
using ConDep.Dsl.Operations.LoadBalancer;
using ConDep.Dsl.SemanticModel;
using ConDep.Dsl.SemanticModel.Sequence;
using ConDep.Dsl.SemanticModel.WebDeploy;
using ConDep.LoadBalancer.Arr;

namespace ConDep.Dsl.LoadBalancer.Arr
{
    //Todo: Use base class instead? In order to guide implementers to provide LoadBalancerSettings in constructor which automatically will be injected.
    public class ApplicationRequestRoutingLoadBalancer : ILoadBalance
    {
        private readonly LoadBalancerConfig _settings;
        private readonly DeploymentUserConfig _user;
        private readonly ServerConfig _server;

        public ApplicationRequestRoutingLoadBalancer(LoadBalancerConfig settings)
        {
            _settings = settings;
            _user = new DeploymentUserConfig { UserName = settings.UserName, Password = settings.Password };
            _server = new ServerConfig { DeploymentUserRemote = _user, Name = _settings.Name };
        }

        public void BringOnline(string serverName, IReportStatus status)
        {
            Execute(LoadBalanceState.Online, serverName, status);
        }

        public void BringOffline(string serverName, LoadBalancerSuspendMethod suspendMethod, IReportStatus status)
        {
            Execute(LoadBalanceState.Offline, serverName, status);
        }

        private void Execute(LoadBalanceState state, string serverName, IReportStatus status)
        {
            //var provider = new ApplicationRequestRoutingProvider(state, serverName);
            
            //var sequence = new ExecutionSequenceManager();
            //var webDeploy = new WebDeployOperator();
            //var server = new RemoteCompositeBuilder(new RemoteSequenceManager(new[] { _server }, null), new[] { _server }, webDeploy);
            //provider.Configure(server);
            //sequence.Execute(status);
        }
    }
}
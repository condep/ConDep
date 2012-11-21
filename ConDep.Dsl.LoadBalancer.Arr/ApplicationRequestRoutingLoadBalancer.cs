using System;
using System.Diagnostics;
using System.Linq;
using ConDep.Dsl;
using ConDep.Dsl.Experimental.Core;
using ConDep.Dsl.LoadBalancer;
using ConDep.Dsl.Model.Config;
using ConDep.Dsl.WebDeploy;

namespace ConDep.LoadBalancer.Arr
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
            _server = new ServerConfig {DeploymentUser = _user, Name = _settings.Name};
        }

        public void BringOnline(string serverName, IReportStatus status)
        {
            var operation = GetOperation(LoadBalanceState.Online, serverName);
            operation.Execute(status);
        }

        public void BringOffline(string serverName, IReportStatus status)
        {
            var operation = GetOperation(LoadBalanceState.Offline, serverName);
            operation.Execute(status);
        }

        private WebDeployOperation GetOperation(LoadBalanceState state, string serverName)
        {
            var webDepDef = CreateWebDeployDefinition();

            var provider = new ApplicationRequestRoutingProvider(state, serverName);
            provider.Configure(_server);
            //Todo: Why is the child providers in the wrong order by default??
            provider.ChildProviders.ToList().Reverse();
            webDepDef.AddProvider(provider, null);

            var webOp = new WebDeployOperation(webDepDef);
            return webOp;
        }

        private WebDeployServerDefinition CreateWebDeployDefinition()
        {
            return  WebDeployServerDefinition.CreateOrGetForServer(_server);
        }
    }
}
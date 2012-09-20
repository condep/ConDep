using System;
using System.Diagnostics;
using System.Linq;
using ConDep.Dsl;
using ConDep.Dsl.LoadBalancer;
using ConDep.Dsl.WebDeploy;

namespace ConDep.LoadBalancer.Arr
{
    //Todo: Use base class instead? In order to guide implementers to provide LoadBalancerSettings in constructor which automatically will be injected.
    public class ApplicationRequestRoutingLoadBalancer : ILoadBalance
    {
        private readonly LoadBalancerSettings _settings;
        private DeploymentUser _user;

        public ApplicationRequestRoutingLoadBalancer(LoadBalancerSettings settings)
        {
            _settings = settings;
            _user = new DeploymentUser {UserName = settings.UserName, Password = settings.Password};
        }

        public void BringOnline(string serverName, TraceLevel traceLevel, EventHandler<WebDeployMessageEventArgs> output, EventHandler<WebDeployMessageEventArgs> outputError, WebDeploymentStatus webDeploymentStatus)
        {
            var operation = GetOperation(LoadBalanceState.Online, serverName);
            operation.Execute(traceLevel, output, outputError, webDeploymentStatus);
        }

        public void BringOffline(string serverName, TraceLevel traceLevel, EventHandler<WebDeployMessageEventArgs> output, EventHandler<WebDeployMessageEventArgs> outputError, WebDeploymentStatus webDeploymentStatus)
        {
            var operation = GetOperation(LoadBalanceState.Offline, serverName);
            operation.Execute(traceLevel, output, outputError, webDeploymentStatus);
        }

        private WebDeployOperation GetOperation(LoadBalanceState state, string serverName)
        {
            var webDepDef = CreateWebDeployDefinition();

            var provider = new ApplicationRequestRoutingProvider(state, serverName);
            provider.Configure(new DeploymentServer(_settings.Name, _user));
            //Todo: Why is the child providers in the wrong order by default??
            provider.ChildProviders.ToList().Reverse();
            webDepDef.Providers.Add(provider);

            var webOp = new WebDeployOperation(webDepDef);
            return webOp;
        }

        private WebDeployServerDefinition CreateWebDeployDefinition()
        {
            var user = new DeploymentUser() {UserName = _settings.UserName, Password = _settings.Password};
            return  WebDeployServerDefinition.CreateOrGetForServer(new DeploymentServer(_settings.Name, user));
        }
    }
}
using System;
using System.Diagnostics;
using ConDep.Dsl.Core;
using ConDep.Dsl.Core.LoadBalancer;

namespace ConDep.Dsl.Infrastructure.Providers.ApplicationRequestRouting
{
    public class ApplicationRequestRoutingLoadBalancer : ILoadBalance
    {
        private readonly string _loadBalancerComputerName;

        public ApplicationRequestRoutingLoadBalancer(string loadBalancerComputerName)
        {
            _loadBalancerComputerName = loadBalancerComputerName;
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

            var provider = new ApplicationRequestRoutingProvider(state);
            provider.Configure(new DeploymentServer(serverName));
            //Todo: Why is the child providers in the wrong order by default??
            provider.ChildProviders.Reverse();
            webDepDef.Providers.Add(provider);

            var webOp = new WebDeployOperation(webDepDef);
            return webOp;
        }

        private WebDeployServerDefinition CreateWebDeployDefinition()
        {
            var webDepDef = new WebDeployServerDefinition();
            //Todo: Add credetials
            webDepDef.WebDeployDestination.ComputerName = _loadBalancerComputerName;
            webDepDef.WebDeploySource.LocalHost = true;

            if (ConDepConfiguratorBase.EnvSettings.LoadBalancer.UserIsDefined)
            {
                webDepDef.WebDeployDestination.Credentials.UserName = ConDepConfiguratorBase.EnvSettings.LoadBalancer.UserName;
                webDepDef.WebDeployDestination.Credentials.Password = ConDepConfiguratorBase.EnvSettings.LoadBalancer.Password;

                //Todo: Use LoadBalancer user or deployment user for Source?
                webDepDef.WebDeploySource.Credentials.UserName = ConDepConfiguratorBase.EnvSettings.LoadBalancer.UserName;
                webDepDef.WebDeploySource.Credentials.Password = ConDepConfiguratorBase.EnvSettings.LoadBalancer.Password;
            }
            return webDepDef;
        }
    }
}
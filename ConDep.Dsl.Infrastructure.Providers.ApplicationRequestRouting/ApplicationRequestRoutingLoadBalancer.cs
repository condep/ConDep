using System;
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

        public void BringOnline(string serverName, EventHandler<WebDeployMessageEventArgs> output, EventHandler<WebDeployMessageEventArgs> outputError, WebDeploymentStatus webDeploymentStatus)
        {
            var operation = GetOperation(LoadBalanceState.Online, serverName);
            operation.Execute(output, outputError, webDeploymentStatus);
        }

        public void BringOffline(string serverName, EventHandler<WebDeployMessageEventArgs> output, EventHandler<WebDeployMessageEventArgs> outputError, WebDeploymentStatus webDeploymentStatus)
        {
            var operation = GetOperation(LoadBalanceState.Offline, serverName);
            operation.Execute(output, outputError, webDeploymentStatus);
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

        private WebDeployDefinition CreateWebDeployDefinition()
        {
            var webDepDef = new WebDeployDefinition();
            //Todo: Add credetials
            webDepDef.WebDeployDestination.ComputerName = _loadBalancerComputerName;
            webDepDef.WebDeploySource.LocalHost = true;

            if (ConDepConfigurator.EnvSettings.LoadBalancer.UserIsDefined)
            {
                webDepDef.WebDeployDestination.Credentials.UserName = ConDepConfigurator.EnvSettings.LoadBalancer.UserName;
                webDepDef.WebDeployDestination.Credentials.Password = ConDepConfigurator.EnvSettings.LoadBalancer.Password;

                //Todo: Use LoadBalancer user or deployment user for Source?
                webDepDef.WebDeploySource.Credentials.UserName = ConDepConfigurator.EnvSettings.LoadBalancer.UserName;
                webDepDef.WebDeploySource.Credentials.Password = ConDepConfigurator.EnvSettings.LoadBalancer.Password;
            }
            return webDepDef;
        }
    }
}
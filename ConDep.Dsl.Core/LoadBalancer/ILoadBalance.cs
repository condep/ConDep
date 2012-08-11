using System;

namespace ConDep.Dsl.Core.LoadBalancer
{
    public interface ILoadBalance
    {
        void BringOffline(string serverName, EventHandler<WebDeployMessageEventArgs> output, EventHandler<WebDeployMessageEventArgs> outputError, WebDeploymentStatus webDeploymentStatus);
        void BringOnline(string serverName, EventHandler<WebDeployMessageEventArgs> output, EventHandler<WebDeployMessageEventArgs> outputError, WebDeploymentStatus webDeploymentStatus);
    }
}
using System;
using System.Diagnostics;
using ConDep.Dsl;
using ConDep.Dsl.WebDeploy;

namespace ConDep.Dsl.LoadBalancer
{
    public class DefaulLoadBalancer : ILoadBalance
    {
        public void BringOffline(string serverName, TraceLevel traceLevel, EventHandler<WebDeployMessageEventArgs> output, EventHandler<WebDeployMessageEventArgs> outputError, WebDeploymentStatus webDeploymentStatus)
        {
            output(this, new WebDeployMessageEventArgs {Level = TraceLevel.Warning, Message = "Warning: No load balancer is used. If this is not by intention, make sure you configure a provider for load balancing."});
        }

        public void BringOnline(string serverName, TraceLevel traceLevel, EventHandler<WebDeployMessageEventArgs> output, EventHandler<WebDeployMessageEventArgs> outputError, WebDeploymentStatus webDeploymentStatus)
        {
            output(this, new WebDeployMessageEventArgs { Level = TraceLevel.Warning, Message = "Warning: No load balancer is used. If this is not by intention, make sure you configure a provider for load balancing." });
        }
    }
}
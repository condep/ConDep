using System;
using System.Diagnostics;

namespace ConDep.Dsl.Core.LoadBalancer
{
    public interface ILoadBalance
    {
        void BringOffline(string serverName, TraceLevel traceLevel, EventHandler<WebDeployMessageEventArgs> output, EventHandler<WebDeployMessageEventArgs> outputError, WebDeploymentStatus webDeploymentStatus);
        void BringOnline(string serverName, TraceLevel traceLevel, EventHandler<WebDeployMessageEventArgs> output, EventHandler<WebDeployMessageEventArgs> outputError, WebDeploymentStatus webDeploymentStatus);
    }
}
using System;
using System.Diagnostics;
using ConDep.Dsl.Core;

namespace ConDep.Dsl.LoadBalancer
{
    public interface ILoadBalance
    {
        void BringOffline(string serverName, TraceLevel traceLevel, EventHandler<WebDeployMessageEventArgs> output, EventHandler<WebDeployMessageEventArgs> outputError, WebDeploymentStatus webDeploymentStatus);
        void BringOnline(string serverName, TraceLevel traceLevel, EventHandler<WebDeployMessageEventArgs> output, EventHandler<WebDeployMessageEventArgs> outputError, WebDeploymentStatus webDeploymentStatus);
    }
}
using System;

namespace ConDep.Dsl.Core.LoadBalancer
{
    public interface ILoadBalance
    {
        void BringOffline(string serverName, EventHandler<WebDeployMessageEventArgs> output);
        void BringOnline(string serverName, EventHandler<WebDeployMessageEventArgs> output);
    }
}
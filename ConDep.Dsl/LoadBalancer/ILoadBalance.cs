using System;
using System.Diagnostics;
using ConDep.Dsl;
using ConDep.Dsl.WebDeploy;

namespace ConDep.Dsl.LoadBalancer
{
    public interface ILoadBalance
    {
        void BringOffline(string serverName, WebDeploymentStatus webDeploymentStatus);
        void BringOnline(string serverName, WebDeploymentStatus webDeploymentStatus);
    }
}
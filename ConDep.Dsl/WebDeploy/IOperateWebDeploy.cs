using System;
using ConDep.Dsl.Config;
using Microsoft.Web.Deployment;

namespace ConDep.Dsl.WebDeploy
{
    public interface IOperateWebDeploy
    {
        WebDeployOptions GetWebDeployOptions(ServerConfig server, EventHandler<DeploymentTraceEventArgs> onTraceMessage);
    }
}
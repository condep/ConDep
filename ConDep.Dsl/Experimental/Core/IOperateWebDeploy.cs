using System;
using ConDep.Dsl.Model.Config;
using ConDep.Dsl.WebDeploy;
using Microsoft.Web.Deployment;

namespace ConDep.Dsl.Experimental.Core
{
    public interface IOperateWebDeploy
    {
        WebDeployOptions GetWebDeployOptions(ServerConfig server, EventHandler<DeploymentTraceEventArgs> onTraceMessage);
    }
}
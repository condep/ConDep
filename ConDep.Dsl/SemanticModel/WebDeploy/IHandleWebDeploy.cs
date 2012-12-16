using System;
using ConDep.Dsl.Config;
using Microsoft.Web.Deployment;

namespace ConDep.Dsl.SemanticModel.WebDeploy
{
    public interface IHandleWebDeploy
    {
        WebDeployOptions GetWebDeployOptions(ServerConfig server, EventHandler<DeploymentTraceEventArgs> onTraceMessage);
        IReportStatus Sync(IProvide provider, WebDeployOptions webDeployOptions, bool continueOnError, IReportStatus status);
    }
}
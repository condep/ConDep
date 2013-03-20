using System;
using ConDep.Dsl.Config;
using Microsoft.Web.Deployment;

namespace ConDep.Dsl.SemanticModel.WebDeploy
{
    public interface IHandleWebDeploy
    {
        void Sync(IProvide provider, ServerConfig server, bool continueOnError, IReportStatus status,
                           EventHandler<DeploymentTraceEventArgs> onTraceMessage);
    }
}
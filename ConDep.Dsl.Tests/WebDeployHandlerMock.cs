using System;
using ConDep.Dsl.Config;
using ConDep.Dsl.SemanticModel;
using ConDep.Dsl.SemanticModel.WebDeploy;
using Microsoft.Web.Deployment;

namespace ConDep.Dsl.Tests
{
    public class WebDeployHandlerMock : IHandleWebDeploy
    {
        public void Sync(IProvide provider, ServerConfig server, bool continueOnError, IReportStatus status, EventHandler<DeploymentTraceEventArgs> onTraceMessage)
        {
        }
    }
}
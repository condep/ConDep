using System;
using ConDep.Dsl.Config;
using ConDep.Dsl.SemanticModel;
using ConDep.Dsl.SemanticModel.WebDeploy;
using Microsoft.Web.Deployment;

namespace ConDep.Dsl.Tests
{
    public class WebDeployHandlerMock : IHandleWebDeploy
    {
        public WebDeployOptions GetWebDeployOptions(ServerConfig server, EventHandler<DeploymentTraceEventArgs> onTraceMessage)
        {
            return new WebDeployOptions("", null, null, null);
        }

        public IReportStatus Sync(IProvide provider, WebDeployOptions webDeployOptions, bool continueOnError, IReportStatus status)
        {
            return status;
        }
    }
}
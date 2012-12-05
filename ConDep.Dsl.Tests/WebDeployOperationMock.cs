using System;
using ConDep.Dsl.Config;
using ConDep.Dsl.SemanticModel.WebDeploy;
using Microsoft.Web.Deployment;

namespace ConDep.Dsl.Tests
{
    public class WebDeployOperationMock : IOperateWebDeploy
    {
        public WebDeployOptions GetWebDeployOptions(ServerConfig server, EventHandler<DeploymentTraceEventArgs> onTraceMessage)
        {
            throw new NotImplementedException();
        }
    }
}
using System;
using System.Diagnostics;
using ConDep.Dsl.Config;
using Microsoft.Web.Deployment;

namespace ConDep.Dsl.SemanticModel.WebDeploy
{
    public class WebDeployOperator : IOperateWebDeploy
    {
        public WebDeployOptions GetWebDeployOptions(ServerConfig server, EventHandler<DeploymentTraceEventArgs> onTraceMessage)
        {
            var webDeploySource = new WebDeploySource { LocalHost = true };
            var webDeployDestination = new WebDeployDestination { ComputerName = server.Name };

            if (server.DeploymentUser != null && server.DeploymentUser.IsDefined)
            {
                webDeployDestination.Credentials.UserName = server.DeploymentUser.UserName;
                webDeployDestination.Credentials.Password = server.DeploymentUser.Password;

                //Todo: Should this user also be used for source?
                webDeploySource.Credentials.UserName = server.DeploymentUser.UserName;
                webDeploySource.Credentials.Password = server.DeploymentUser.Password;
            }

            var syncOptions = new DeploymentSyncOptions();// { WhatIf = Configuration.UseWhatIf };

            var sourceBaseOptions = webDeploySource.GetSourceBaseOptions();
            sourceBaseOptions.TempAgent = false;
            sourceBaseOptions.Trace += onTraceMessage;
            sourceBaseOptions.TraceLevel = TraceLevel.Verbose;

            var destBaseOptions = webDeployDestination.GetDestinationBaseOptions();
            destBaseOptions.TempAgent = false;
            destBaseOptions.Trace += onTraceMessage;
            destBaseOptions.TraceLevel = TraceLevel.Verbose;

            return new WebDeployOptions(webDeploySource.PackagePath, sourceBaseOptions, destBaseOptions, syncOptions);
        }
    }
}
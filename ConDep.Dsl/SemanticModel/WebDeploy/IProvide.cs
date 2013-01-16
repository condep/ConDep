using System;
using System.Collections.Generic;
using Microsoft.Web.Deployment;

namespace ConDep.Dsl.SemanticModel.WebDeploy
{
    public interface IProvide
    {
        bool IsValid(Notification notification);
		int WaitIntervalInSeconds { get; set; }
        int RetryAttempts { get; set; }
        bool ContinueOnError { get; set; }
        DeploymentProviderOptions GetWebDeploySourceProviderOptions();
        DeploymentProviderOptions GetWebDeployDestinationProviderOptions();
        DeploymentBaseOptions GetWebDeploySourceBaseOptions();
        DeploymentBaseOptions GetWebDeployDestBaseOptions();
        IList<DeploymentRule> GetReplaceRules();
    }
}
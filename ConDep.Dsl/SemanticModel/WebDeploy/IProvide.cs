using System;
using System.Collections.Generic;
using Microsoft.Web.Deployment;

namespace ConDep.Dsl.SemanticModel.WebDeploy
{
    public interface IProvide
    {
        bool IsValid(Notification notification);
		int WaitInterval { get; set; }
        DeploymentObject GetWebDeploySourceObject(DeploymentBaseOptions sourceBaseOptions);
        DeploymentProviderOptions GetWebDeployDestinationObject();
        IList<DeploymentRule> GetReplaceRules();
    }
}
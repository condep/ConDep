using System;
using ConDep.Dsl.FluentWebDeploy.SemanticModel;

namespace ConDep.Dsl.FluentWebDeploy.Deployment
{
	public interface IWebDeploy
	{
        DeploymentStatus Deploy(WebDeployDefinition webDeployDefinition, Action<object, WebDeployMessageEventArgs> output, Action<object, WebDeployMessageEventArgs> outputError);
	}
}
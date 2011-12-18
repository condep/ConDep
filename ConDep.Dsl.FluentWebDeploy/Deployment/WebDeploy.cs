using System;
using ConDep.Dsl.FluentWebDeploy.SemanticModel;

namespace ConDep.Dsl.FluentWebDeploy.Deployment
{
	public class WebDeploy : IWebDeploy
	{
		public DeploymentStatus Deploy(WebDeployDefinition webDeployDefinition, Action<object, WebDeployMessageEventArgs> output, Action<object, WebDeployMessageEventArgs> outputError)
		{
		    var sync = new WebDeploySync(webDeployDefinition, output, outputError);
		    return sync.Execute();
		}
	}
}
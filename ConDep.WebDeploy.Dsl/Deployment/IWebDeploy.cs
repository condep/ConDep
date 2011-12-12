using System;
using ConDep.Dsl.FluentWebDeploy.SemanticModel;

namespace ConDep.Dsl.FluentWebDeploy.Deployment
{
	public interface IWebDeploy
	{
		event EventHandler<WebDeployMessageEventArgs> Output;
		event EventHandler<WebDeployMessageEventArgs> OutputError;

		void Deploy(WebDeployDefinition webDeployDefinition);
		void Delete(WebDeployDefinition webDeployDefinition);
	}
}
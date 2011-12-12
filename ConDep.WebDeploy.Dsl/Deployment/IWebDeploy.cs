using System;
using ConDep.WebDeploy.Dsl.SemanticModel;

namespace ConDep.WebDeploy.Dsl.Deployment
{
	public interface IWebDeploy
	{
		event EventHandler<WebDeployMessageEventArgs> Output;
		event EventHandler<WebDeployMessageEventArgs> OutputError;

		void Deploy(WebDeployDefinition webDeployDefinition);
		void Delete(WebDeployDefinition webDeployDefinition);
	}
}
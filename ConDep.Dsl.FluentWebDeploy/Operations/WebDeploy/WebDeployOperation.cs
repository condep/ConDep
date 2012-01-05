using System;
using ConDep.Dsl.FluentWebDeploy.Builders;
using ConDep.Dsl.FluentWebDeploy.Operations.WebDeploy.Model;
using ConDep.Dsl.FluentWebDeploy.SemanticModel;

namespace ConDep.Dsl.FluentWebDeploy.Operations.WebDeploy
{
	public class WebDeployOperation : IOperateConDep
	{
		private readonly WebDeployDefinition _webDeployDefinition;

		public WebDeployOperation(WebDeployDefinition webDeployDefinition)
		{
			_webDeployDefinition = webDeployDefinition;
		}

		public WebDeploymentStatus Execute(EventHandler<WebDeployMessageEventArgs> output, EventHandler<WebDeployMessageEventArgs> outputError, WebDeploymentStatus webDeploymentStatus)
		{
			return _webDeployDefinition.Sync(output, outputError, webDeploymentStatus);
		}

		public bool IsValid(Notification notification)
		{
			throw new NotImplementedException();
		}
	}
}
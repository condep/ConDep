using System;
using ConDep.Dsl.Builders;
using ConDep.Dsl.Operations.WebDeploy.Model;

namespace ConDep.Dsl.Operations.WebDeploy
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
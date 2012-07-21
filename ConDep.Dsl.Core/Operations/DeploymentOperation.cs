using System;

namespace ConDep.Dsl.Core
{
	public class DeploymentOperation : IOperateConDep
	{
		private readonly WebDeployDefinition _webDeployDefinition;

		public DeploymentOperation(WebDeployDefinition webDeployDefinition)
		{
			_webDeployDefinition = webDeployDefinition;
		}

		public WebDeploymentStatus Execute(EventHandler<WebDeployMessageEventArgs> output, EventHandler<WebDeployMessageEventArgs> outputError, WebDeploymentStatus webDeploymentStatus)
		{
			return _webDeployDefinition.Sync(output, outputError, webDeploymentStatus);
		}

		public bool IsValid(Notification notification)
		{
			return _webDeployDefinition.IsValid(notification);
		}
	}
}
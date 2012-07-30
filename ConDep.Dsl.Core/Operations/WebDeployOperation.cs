using System;

namespace ConDep.Dsl.Core
{
	public class WebDeployOperation : ConDepOperation, IRequireLoadBalancing
	{
		private readonly WebDeployDefinition _webDeployDefinition;

		public WebDeployOperation(WebDeployDefinition webDeployDefinition)
		{
			_webDeployDefinition = webDeployDefinition;
		}

		public override WebDeploymentStatus Execute(EventHandler<WebDeployMessageEventArgs> output, EventHandler<WebDeployMessageEventArgs> outputError, WebDeploymentStatus webDeploymentStatus)
		{
            if(BeforeExecute != null)
            {
                BeforeExecute(_webDeployDefinition.WebDeployDestination.ComputerName, output);
            }

            var status = _webDeployDefinition.Sync(output, outputError, webDeploymentStatus);

            if (AfterExecute != null)
            {
                AfterExecute(_webDeployDefinition.WebDeployDestination.ComputerName, output);
            }
		    return status;
		}

		public override bool IsValid(Notification notification)
		{
			return _webDeployDefinition.IsValid(notification);
		}
    }
}
using System;
using System.Diagnostics;

namespace ConDep.Dsl.Core
{
	public class WebDeployOperation : ConDepOperationBase, IRequireLoadBalancing
	{
		private readonly WebDeployServerDefinition _webDeployServerDefinition;

		public WebDeployOperation(WebDeployServerDefinition webDeployServerDefinition)
		{
			_webDeployServerDefinition = webDeployServerDefinition;
		}

		public override WebDeploymentStatus Execute(TraceLevel traceLevel, EventHandler<WebDeployMessageEventArgs> output, EventHandler<WebDeployMessageEventArgs> outputError, WebDeploymentStatus webDeploymentStatus)
		{
		    _webDeployServerDefinition.TraceLevel = traceLevel;

            if(BeforeExecute != null)
            {
                BeforeExecute(_webDeployServerDefinition.WebDeployDestination.ComputerName, traceLevel, output, outputError, webDeploymentStatus);
            }

            var status = _webDeployServerDefinition.Sync(output, outputError, webDeploymentStatus);

            if (AfterExecute != null)
            {
                AfterExecute(_webDeployServerDefinition.WebDeployDestination.ComputerName, traceLevel, output, outputError, webDeploymentStatus);
            }
		    return status;
		}

		public override bool IsValid(Notification notification)
		{
			return _webDeployServerDefinition.IsValid(notification);
		}
    }
}
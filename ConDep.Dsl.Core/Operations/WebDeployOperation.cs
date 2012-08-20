using System;
using System.Diagnostics;

namespace ConDep.Dsl.Core
{
	public class WebDeployOperation : ConDepOperation, IRequireLoadBalancing
	{
		private readonly WebDeployDefinition _webDeployDefinition;

		public WebDeployOperation(WebDeployDefinition webDeployDefinition)
		{
			_webDeployDefinition = webDeployDefinition;
		}

		public override WebDeploymentStatus Execute(TraceLevel traceLevel, EventHandler<WebDeployMessageEventArgs> output, EventHandler<WebDeployMessageEventArgs> outputError, WebDeploymentStatus webDeploymentStatus)
		{
		    _webDeployDefinition.TraceLevel = traceLevel;

            if(BeforeExecute != null)
            {
                BeforeExecute(_webDeployDefinition.WebDeployDestination.ComputerName, traceLevel, output, outputError, webDeploymentStatus);
            }

            var status = _webDeployDefinition.Sync(output, outputError, webDeploymentStatus);

            if (AfterExecute != null)
            {
                AfterExecute(_webDeployDefinition.WebDeployDestination.ComputerName, traceLevel, output, outputError, webDeploymentStatus);
            }
		    return status;
		}

		public override bool IsValid(Notification notification)
		{
			return _webDeployDefinition.IsValid(notification);
		}

	    public WebDeployDefinition WebDeployDefinition
	    {
            get { return _webDeployDefinition; }
	    }
    }
}
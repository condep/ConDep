using System;
using System.Diagnostics;
using System.IO;

namespace ConDep.Dsl.WebDeploy
{
	public class WebDeployOperation : ConDepOperationBase, IRequireLoadBalancing
	{
		private readonly WebDeployServerDefinition _webDeployServerDefinition;

		public WebDeployOperation(WebDeployServerDefinition webDeployServerDefinition)
		{
			_webDeployServerDefinition = webDeployServerDefinition;
		}

		public override WebDeploymentStatus Execute(WebDeploymentStatus webDeploymentStatus)
		{
		    _webDeployServerDefinition.TraceLevel = Logger.TraceLevel;

            if(BeforeExecute != null)
            {
                BeforeExecute(_webDeployServerDefinition.WebDeployDestination.ComputerName, webDeploymentStatus);
                if(webDeploymentStatus.HasErrors) return webDeploymentStatus;
            }

            var status = _webDeployServerDefinition.Sync(webDeploymentStatus);

            if (webDeploymentStatus.HasErrors) return webDeploymentStatus;

            if (AfterExecute != null)
            {
                AfterExecute(_webDeployServerDefinition.WebDeployDestination.ComputerName, webDeploymentStatus);
            }
		    return status;
		}

		public override bool IsValid(Notification notification)
		{
			return _webDeployServerDefinition.IsValid(notification);
		}

	    public override void PrintExecutionSequence(TextWriter writer)
	    {
            foreach (var provider in _webDeployServerDefinition.Providers)
	        {
	            writer.WriteLine(provider.GetType().Name);
	        }
	    }
	}
}
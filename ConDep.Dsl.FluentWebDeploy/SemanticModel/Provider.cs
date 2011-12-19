using System;
using ConDep.Dsl.FluentWebDeploy.Deployment;
using Microsoft.Web.Deployment;

namespace ConDep.Dsl.FluentWebDeploy.SemanticModel
{
	public abstract class Provider : IProvide, IWebDeployModel
	{
        private Exception _untrappedExitCodeException;
        
        public string SourcePath { get; set; }
		public virtual string DestinationPath { get; set; }
		public abstract string Name { get; }
		public int WaitInterval { get; set; }

		public abstract DeploymentProviderOptions GetWebDeployDestinationObject();
		public abstract DeploymentObject GetWebDeploySourceObject(DeploymentBaseOptions sourceBaseOptions);
		public abstract bool IsValid(Notification notification);

        public DeploymentStatus Sync(WebDeployOptions webDeployOptions, DeploymentStatus deploymentStatus)
        {
            try
            {
                webDeployOptions.DestBaseOptions.Trace += CheckForUntrappedRunCommandExitCodes;
                var defaultWaitInterval = webDeployOptions.DestBaseOptions.RetryInterval;

                if (WaitInterval > 0)
                {
                    webDeployOptions.DestBaseOptions.RetryInterval = WaitInterval * 1000;
                }

                var sourceDepObject = GetWebDeploySourceObject(webDeployOptions.SourceBaseOptions);
                var destProviderOptions = GetWebDeployDestinationObject();

                var summery = sourceDepObject.SyncTo(destProviderOptions, webDeployOptions.DestBaseOptions, webDeployOptions.SyncOptions);

                deploymentStatus.AddSummery(summery);

                webDeployOptions.DestBaseOptions.RetryInterval = defaultWaitInterval;

                if (summery.Errors > 0)
                {
                    throw new Exception("The provider reported " + summery.Errors + " during deployment.");
                }

                if (_untrappedExitCodeException != null)
                {
                    throw _untrappedExitCodeException;
                }
            }
            finally
            {
                webDeployOptions.DestBaseOptions.Trace -= CheckForUntrappedRunCommandExitCodes;
            }

            return deploymentStatus;
        }

        void CheckForUntrappedRunCommandExitCodes(object sender, DeploymentTraceEventArgs e)
        {
            //Terrible hack to trap exit codes that the WebDeploy runCommand ignores!
            if (e.Message.Contains("exited with code "))
            {
                if (!e.Message.Contains("exited with code '0x0'"))
                {
                    _untrappedExitCodeException = new Exception(e.Message, _untrappedExitCodeException);
                }
            }
        }

	}
}
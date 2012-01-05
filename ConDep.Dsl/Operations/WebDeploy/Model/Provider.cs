using System;
using Microsoft.Web.Deployment;

namespace ConDep.Dsl.Operations.WebDeploy.Model
{
	public abstract class Provider : IProvide, IValidate
	{
        public string SourcePath { get; set; }
		public virtual string DestinationPath { get; set; }
		public abstract string Name { get; }
		public int WaitInterval { get; set; }

		public abstract DeploymentProviderOptions GetWebDeployDestinationObject();
		public abstract DeploymentObject GetWebDeploySourceObject(DeploymentBaseOptions sourceBaseOptions);
		public abstract bool IsValid(Notification notification);

        public virtual WebDeploymentStatus Sync(WebDeployOptions webDeployOptions, WebDeploymentStatus deploymentStatus)
        {
            var defaultWaitInterval = webDeployOptions.DestBaseOptions.RetryInterval;

            if (WaitInterval > 0)
            {
                webDeployOptions.DestBaseOptions.RetryInterval = WaitInterval * 1000;
            }

            DeploymentChangeSummary summery;
            using (var sourceDepObject = webDeployOptions.FromPackage ? GetPackageSourceObject(webDeployOptions) : GetWebDeploySourceObject(webDeployOptions.SourceBaseOptions))
            {
                var destProviderOptions = GetWebDeployDestinationObject();

                //if(webDeployOptions.FromPackage)
                //{
                //    summery = sourceDepObject.SyncTo(DeploymentWellKnownProvider.Auto, "", webDeployOptions.DestBaseOptions, webDeployOptions.SyncOptions);
                //}
                //else
                //{
                    summery = sourceDepObject.SyncTo(destProviderOptions, webDeployOptions.DestBaseOptions, webDeployOptions.SyncOptions);
                //}
            }

            deploymentStatus.AddSummery(summery);

            webDeployOptions.DestBaseOptions.RetryInterval = defaultWaitInterval;

            if (summery.Errors > 0)
            {
                throw new Exception("The provider reported " + summery.Errors + " during deployment.");
            }
            return deploymentStatus;
        }

	    private static DeploymentObject GetPackageSourceObject(WebDeployOptions webDeployOptions)
	    {
            return DeploymentManager.CreateObject(DeploymentWellKnownProvider.Package, webDeployOptions.PackagePath, webDeployOptions.SourceBaseOptions);
	    }
	}
}
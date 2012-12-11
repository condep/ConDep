using System.Collections.Generic;
using System.Linq;
using Microsoft.Web.Deployment;

namespace ConDep.Dsl.SemanticModel.WebDeploy
{
	public abstract class WebDeployProviderBase : IProvide, IValidate
	{
        public string SourcePath { get; set; }
		public virtual string DestinationPath { get; set; }
		public abstract string Name { get; }
        public int WaitInterval { get; set; }
        public int RetryAttempts { get; set; }

	    public abstract DeploymentProviderOptions GetWebDeployDestinationObject();
		public abstract DeploymentObject GetWebDeploySourceObject(DeploymentBaseOptions sourceBaseOptions);
        public virtual IList<DeploymentRule> GetReplaceRules() { return new List<DeploymentRule>(); }

		public abstract bool IsValid(Notification notification);
	 
        public virtual IReportStatus Sync(WebDeployOptions webDeployOptions, IReportStatus status)
        {
            var defaultWaitInterval = webDeployOptions.DestBaseOptions.RetryInterval;
            var defaultRetryAttempts = webDeployOptions.DestBaseOptions.RetryAttempts;

            if (WaitInterval > 0)
            {
                webDeployOptions.DestBaseOptions.RetryInterval = WaitInterval * 1000;
            }

            if(RetryAttempts > 0)
            {
                webDeployOptions.DestBaseOptions.RetryAttempts = RetryAttempts;
            }

            DeploymentChangeSummary summery;
            using (var sourceDepObject = webDeployOptions.FromPackage ? GetPackageSourceObject(webDeployOptions) : GetWebDeploySourceObject(webDeployOptions.SourceBaseOptions))
            {
                var destProviderOptions = GetWebDeployDestinationObject();

                foreach (var rule in GetReplaceRules())
                {
                    webDeployOptions.SyncOptions.Rules.Add(rule);
                }
                summery = sourceDepObject.SyncTo(destProviderOptions, webDeployOptions.DestBaseOptions, webDeployOptions.SyncOptions);
            }

            status.AddSummery(summery);

            webDeployOptions.DestBaseOptions.RetryInterval = defaultWaitInterval;
            webDeployOptions.DestBaseOptions.RetryAttempts = defaultRetryAttempts;

            if (summery.Errors > 0)
            {
                throw new ConDepWebDeployProviderException("The provider reported " + summery.Errors + " during deployment.");
            }
            return status;
        }

	    private void Backup(WebDeployOptions webDeployOptions)
	    {
	        bool existOnDestination = true;
            try
            {
                var dest = DeploymentManager.CreateObject(Name, DestinationPath, webDeployOptions.DestBaseOptions);
                var children = dest.GetChildren();
                if (children.Count() == 0)
                    existOnDestination = false;
            }
            catch
            {
                existOnDestination = false;
            }

            if(existOnDestination)
            {
                var backupSource = DeploymentManager.CreateObject(Name, DestinationPath, webDeployOptions.DestBaseOptions);
                backupSource.SyncTo(DeploymentWellKnownProvider.Package, @"C:\Temp\PackageBackup.zip",
                                    webDeployOptions.DestBaseOptions, webDeployOptions.SyncOptions);
            }
	    }

	    private static DeploymentObject GetPackageSourceObject(WebDeployOptions webDeployOptions)
	    {
            return DeploymentManager.CreateObject(DeploymentWellKnownProvider.Package, webDeployOptions.PackagePath, webDeployOptions.SourceBaseOptions);
	    }
	}
}
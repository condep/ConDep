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
        public int WaitIntervalInSeconds { get; set; }
        public int RetryAttempts { get; set; }

	    public abstract DeploymentProviderOptions GetWebDeployDestinationObject();
		public abstract DeploymentObject GetWebDeploySourceObject(DeploymentBaseOptions sourceBaseOptions);
        public virtual IList<DeploymentRule> GetReplaceRules() { return new List<DeploymentRule>(); }

		public abstract bool IsValid(Notification notification);
	 
        //private void Backup(WebDeployOptions webDeployOptions)
        //{
        //    bool existOnDestination = true;
        //    try
        //    {
        //        var dest = DeploymentManager.CreateObject(Name, DestinationPath, webDeployOptions.DestBaseOptions);
        //        var children = dest.GetChildren();
        //        if (children.Count() == 0)
        //            existOnDestination = false;
        //    }
        //    catch
        //    {
        //        existOnDestination = false;
        //    }

        //    if(existOnDestination)
        //    {
        //        var backupSource = DeploymentManager.CreateObject(Name, DestinationPath, webDeployOptions.DestBaseOptions);
        //        backupSource.SyncTo(DeploymentWellKnownProvider.Package, @"C:\Temp\PackageBackup.zip",
        //                            webDeployOptions.DestBaseOptions, webDeployOptions.SyncOptions);
        //    }
        //}
	}
}
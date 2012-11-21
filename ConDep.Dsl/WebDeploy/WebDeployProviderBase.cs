using System.Collections.Generic;
using System.Linq;
using ConDep.Dsl.Experimental.Core;
using Microsoft.Web.Deployment;

namespace ConDep.Dsl.WebDeploy
{
	public abstract class WebDeployProviderBase : IProvide, IValidate
	{
	    private readonly List<IProvideConditions> _conditions = new List<IProvideConditions>();

        public string SourcePath { get; set; }
		public virtual string DestinationPath { get; set; }
		public abstract string Name { get; }
		public int WaitInterval { get; set; }

	    public abstract DeploymentProviderOptions GetWebDeployDestinationObject();
		public abstract DeploymentObject GetWebDeploySourceObject(DeploymentBaseOptions sourceBaseOptions);
        public virtual IList<DeploymentRule> GetReplaceRules() { return new List<DeploymentRule>(); }

		public abstract bool IsValid(Notification notification);

	    public void AddCondition(IProvideConditions condition)
	    {
	        _conditions.Add(condition);
	    }

        public virtual IReportStatus Sync(WebDeployOptions webDeployOptions, IReportStatus status)
        {
            if (HasConditions())
            {
                if (_conditions.Any(condition => !condition.HasExpectedOutcome(webDeployOptions))) return status;
            }

            var defaultWaitInterval = webDeployOptions.DestBaseOptions.RetryInterval;

            if (WaitInterval > 0)
            {
                webDeployOptions.DestBaseOptions.RetryInterval = WaitInterval * 1000;
            }

            DeploymentChangeSummary summery;
            using (var sourceDepObject = webDeployOptions.FromPackage ? GetPackageSourceObject(webDeployOptions) : GetWebDeploySourceObject(webDeployOptions.SourceBaseOptions))
            {
                var destProviderOptions = GetWebDeployDestinationObject();

                foreach (var rule in GetReplaceRules())
                {
                    webDeployOptions.SyncOptions.Rules.Add(rule);
                }

                //if(webDeployOptions.FromPackage)
                //{
                //    summery = sourceDepObject.SyncTo(DeploymentWellKnownProvider.Auto, "", webDeployOptions.DestBaseOptions, webDeployOptions.SyncOptions);
                //}
                //else
                //{

                //Backup(webDeployOptions);
                foreach(var factory in DeploymentManager.ProviderFactories)
                {
                }
                summery = sourceDepObject.SyncTo(destProviderOptions, webDeployOptions.DestBaseOptions, webDeployOptions.SyncOptions);
                //}
            }

            status.AddSummery(summery);

            webDeployOptions.DestBaseOptions.RetryInterval = defaultWaitInterval;

            if (summery.Errors > 0)
            {
                throw new ConDepWebDeployProviderException("The provider reported " + summery.Errors + " during deployment.");
            }
            return status;
        }

	    private bool HasConditions()
	    {
	        return _conditions.Count > 0;
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

    //public class WebDeployProviderCollection : DeploymentObjectProvider
    //{
    //    public WebDeployProviderCollection(DeploymentBaseContext baseContext) : base(baseContext)
    //    {
    //    }

    //    public WebDeployProviderCollection(DeploymentProviderContext providerContext, DeploymentBaseContext baseContext) : base(providerContext, baseContext)
    //    {
    //    }

    //    public override DeploymentObjectAttributeData CreateKeyAttributeData()
    //    {
    //        throw new System.NotImplementedException();
    //    }

    //    public override string Name
    //    {
    //        get { throw new System.NotImplementedException(); }
    //    }

    //    public override DeploymentObjectProvider AddChild(DeploymentObject source, int position, bool whatIf)
    //    {
    //        string factoryName = (string)null;
    //        string factoryPath = (string)null;
    //        source.GetFactoryInfo(out factoryName, out factoryPath);
    //        if (this.BaseContext.SourceObject != null)
    //        {
    //            foreach (DeploymentSyncParameter deploymentSyncParameter in this.BaseContext.SourceObject.SyncParameters)
    //            {
    //                foreach (DeploymentSyncParameterEntry syncParameterEntry in deploymentSyncParameter.Entries)
    //                {
    //                    if (syncParameterEntry.Kind == DeploymentSyncParameterEntryKind.ProviderPath && syncParameterEntry.IsScopeMatch(factoryName) && (syncParameterEntry.Match == null || RegexHelper.IsMatch(factoryPath, syncParameterEntry.Match)))
    //                        factoryPath = deploymentSyncParameter.Value;
    //                }
    //            }
    //        }
    //        DeploymentProviderOptions providerOptions = source.ProviderContext == null ? new DeploymentProviderOptions(factoryName) : new DeploymentProviderOptions(factoryName, (IEnumerable<DeploymentProviderSetting>)source.ProviderContext.ProviderSettings);
    //        providerOptions.Path = factoryPath;
    //        DeploymentProviderContext providerContext = new DeploymentProviderContext(providerOptions);
    //        return providerOptions.Factory.CreateProvider(providerContext, this.BaseContext);
    //    }
    //}
}
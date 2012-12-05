using System.IO;
using ConDep.Dsl.SemanticModel;
using ConDep.Dsl.SemanticModel.WebDeploy;
using Microsoft.Web.Deployment;

namespace ConDep.Dsl.Operations.Application.Deployment.WebApp
{
	public class WebAppDeploymentProvider : WebDeployProviderBase
	{
		private const string NAME = "iisApp";

		public WebAppDeploymentProvider(string sourceDir, string webAppName, string destinationWebSiteName)
		{
			SourcePath = Path.GetFullPath(sourceDir);
		    DestinationAppName = webAppName;
		    DestinationWebSite = destinationWebSiteName;
		}

        public WebAppDeploymentProvider(string sourceWebSiteName, string sourceWebAppName, string destinationWebSiteName, string destinationWebAppName)
        {
            SourcePath = string.Format("{0}/{1}", sourceWebSiteName, sourceWebAppName);
            DestinationAppName = destinationWebAppName;
            DestinationWebSite = destinationWebSiteName;
        }

	    public string DestinationWebSite { get; set; }
		public string DestinationAppName { get; set; }

		public override string DestinationPath
		{
			get
			{
				return DestinationWebSite + "/" + DestinationAppName;
			}
		}

		public override string Name
		{
			get { return NAME; }
		}

		public override DeploymentProviderOptions GetWebDeployDestinationObject()
		{
			return new DeploymentProviderOptions(Name) { Path = DestinationPath };
		}

		public override DeploymentObject GetWebDeploySourceObject(DeploymentBaseOptions sourceBaseOptions)
		{
			return DeploymentManager.CreateObject(Name, SourcePath, sourceBaseOptions);
		}

		public override bool IsValid(Notification notification)
		{
			return !string.IsNullOrWhiteSpace(SourcePath) && 
					 !string.IsNullOrWhiteSpace(DestinationWebSite) &&
			       !string.IsNullOrWhiteSpace(DestinationAppName);
		}
	}
}
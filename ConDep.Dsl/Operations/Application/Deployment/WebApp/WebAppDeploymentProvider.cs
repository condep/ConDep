using System.IO;
using ConDep.Dsl.Config;
using ConDep.Dsl.Logging;
using ConDep.Dsl.SemanticModel;
using ConDep.Dsl.SemanticModel.WebDeploy;
using ConDep.Node.Client;
using Microsoft.Web.Deployment;

namespace ConDep.Dsl.Operations.Application.Deployment.WebApp
{
    public class WebAppOperation : IOperateRemote
    {
        private readonly string _sourceDir;
        private readonly string _webAppName;
        private readonly string _destinationWebSiteName;
        private readonly string _destDir;
        private Api _api;

        public WebAppOperation(string sourceDir, string webAppName, string destinationWebSiteName, string destDir = null)
        {
            _sourceDir = sourceDir;
            _webAppName = webAppName;
            _destinationWebSiteName = destinationWebSiteName;
            _destDir = destDir;
        }

        public bool IsValid(Notification notification)
        {
            return true;
        }

        public void Execute(ServerConfig server, IReportStatus status, ConDepSettings settings)
        {
            _api = new Api(string.Format("http://{0}/ConDepNode/", server.Name));
            var result = _api.SyncWebApp(_destinationWebSiteName, _webAppName, _sourceDir, _destDir);
            Logger.Info(
@"Sync result:

    Files Created       : {0}
    Files Updated       : {3}
    Files Deleted       : {2}
    Directories Deleted : {1}
", result.CreatedFiles, result.DeletedDirectories, result.DeletedFiles, result.UpdatedFiles);

        }
    }

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

	    public override DeploymentProviderOptions GetWebDeploySourceProviderOptions()
	    {
            return new DeploymentProviderOptions(NAME) { Path = SourcePath };

	    }

	    public override DeploymentProviderOptions GetWebDeployDestinationProviderOptions()
		{
			return new DeploymentProviderOptions(Name) { Path = DestinationPath };
		}

		public override bool IsValid(Notification notification)
		{
			return !string.IsNullOrWhiteSpace(SourcePath) && 
					 !string.IsNullOrWhiteSpace(DestinationWebSite) &&
			       !string.IsNullOrWhiteSpace(DestinationAppName);
		}
	}
}
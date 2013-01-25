using System.IO;
using ConDep.Dsl.SemanticModel;
using ConDep.Dsl.SemanticModel.WebDeploy;
using Microsoft.Web.Deployment;

namespace ConDep.Dsl.Operations.Application.Deployment.CopyDir
{
	public class CopyDirProvider : WebDeployProviderBase
	{
	    private const string NAME = "dirPath";

		public CopyDirProvider(string sourceDir, string destDir)
		{
		    SourcePath = !Path.IsPathRooted(sourceDir) ? Path.GetFullPath(sourceDir) : sourceDir;
		    DestinationPath = destDir;
		}

	    public override string Name
		{
			get { return NAME; }
		}

	    public override DeploymentProviderOptions GetWebDeploySourceProviderOptions()
	    {
            var sourceProviderOptions = new DeploymentProviderOptions(Name) { Path = SourcePath };

            DeploymentProviderSetting waitAttempts;
            DeploymentProviderSetting waitInterval;

            if (sourceProviderOptions.ProviderSettings.TryGetValue("waitAttempts", out waitAttempts))
            {
                waitAttempts.Value = 10;
            }
            if (sourceProviderOptions.ProviderSettings.TryGetValue("waitInterval", out waitInterval))
            {
                waitInterval.Value = 5000;
            }
            return sourceProviderOptions;
        }

        public override DeploymentProviderOptions GetWebDeployDestinationProviderOptions()
        {
            var destProviderOptions = new DeploymentProviderOptions(Name) { Path = DestinationPath };

            DeploymentProviderSetting waitAttempts;
            DeploymentProviderSetting waitInterval;

            if (destProviderOptions.ProviderSettings.TryGetValue("waitAttempts", out waitAttempts))
            {
                waitAttempts.Value = 10;
            }
            if (destProviderOptions.ProviderSettings.TryGetValue("waitInterval", out waitInterval))
            {
                waitInterval.Value = 5000;
            }
            return destProviderOptions;
        }

        public override bool IsValid(Notification notification)
		{
			var valid = true;

			if (string.IsNullOrWhiteSpace(SourcePath))
			{
				notification.AddError(new SemanticValidationError(string.Format("Source path is missing for provider <{0}>.", GetType().Name), ValidationErrorType.NoSourcePathForProvider));
				valid = false;
			}

			if(string.IsNullOrWhiteSpace(DestinationPath))
			{
				notification.AddError(new SemanticValidationError(string.Format("Destination path is missing for provider <{0}>.", GetType().Name), ValidationErrorType.NoDestinationPathForProvider));
				valid = false;
			}
			return valid;
		}
	}
}

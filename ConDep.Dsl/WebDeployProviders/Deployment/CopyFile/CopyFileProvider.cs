using ConDep.Dsl.Core;
using Microsoft.Web.Deployment;
using DeploymentProviderOptions = Microsoft.Web.Deployment.DeploymentProviderOptions;

namespace ConDep.Dsl
{
	public class CopyFileProvider : WebDeployProviderBase
	{
		private const string NAME = "filePath";

		public CopyFileProvider(string sourcePath)
		{
			SourcePath = sourcePath;
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
			var valid = true;

			if (string.IsNullOrWhiteSpace(SourcePath))
			{
				notification.AddError(new SemanticValidationError(string.Format("Source path is missing for provider <{0}>.", GetType().Name), ValidationErrorType.NoSourcePathForProvider));
				valid = false;
			}

			if (string.IsNullOrWhiteSpace(DestinationPath))
			{
				notification.AddError(new SemanticValidationError(string.Format("Destination path is missing for provider <{0}>.", GetType().Name), ValidationErrorType.NoDestinationPathForProvider));
				valid = false;
			}
			return valid;
		}
	}
}
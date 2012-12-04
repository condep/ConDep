using System.IO;
using ConDep.Dsl.SemanticModel;
using ConDep.Dsl.WebDeploy;
using Microsoft.Web.Deployment;

namespace ConDep.Dsl.Operations.Application.Deployment.CopyFile
{
	public class CopyFileProvider : WebDeployProviderBase
	{
		private const string NAME = "filePath";

		public CopyFileProvider(string sourceFilePath, string destFilepath)
		{
            SourcePath = !Path.IsPathRooted(sourceFilePath) ? Path.GetFullPath(sourceFilePath) : sourceFilePath; ;
		    DestinationPath = destFilepath;
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
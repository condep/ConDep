using System;
using ConDep.WebDeploy.Dsl.SemanticModel;
using Microsoft.Web.Deployment;

namespace ConDep.WebDeploy.Dsl
{
	public class CopyDirProvider : Provider
	{
		private const string NAME = "dirPath";

		public CopyDirProvider(string sourcePath)
		{
			SourcePath = sourcePath;
			Name = NAME;
		}

		public override DeploymentProviderOptions GetWebDeployDestinationProviderOptions()
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
				notification.AddError(new SemanticValidationError(string.Format("Source path is missing for provider <{0}>.", GetType().Name), ValidationErrorType.NoSourceForProvider));
				valid = false;
			}

			if(string.IsNullOrWhiteSpace(DestinationPath))
			{
				notification.AddError(new SemanticValidationError(string.Format("Source path is missing for provider <{0}>.", GetType().Name), ValidationErrorType.NoDestinationForProvider));
				valid = false;
			}
			return valid;
		}
	}

	public class MissingProviderDestinationException : Exception
	{
		public MissingProviderDestinationException(string message) : base(message)
		{
			
		}
	}

	public class MissingProviderSourceException : Exception
	{
		public MissingProviderSourceException(string message) : base(message)
		{
			
		}
	}
}

using System.Collections.Generic;
using ConDep.WebDeploy.Dsl;
using Microsoft.Web.Deployment;

namespace ConDep.WebDeploy.Dsl.SemanticModel
{
	public class Source : IWebDeployModel
	{
		private CredentialsProvider _credentialsProvider = new CredentialsProvider();

		public string ComputerName { get; set; }
		public bool LocalHost { get; set; }
		public bool HasCredentials
		{
			get { return !string.IsNullOrWhiteSpace(CredentialsProvider.UserName); }
		}

		public CredentialsProvider CredentialsProvider
		{
			get {
				return _credentialsProvider;
			}
		}

		public DeploymentBaseOptions GetSourceBaseOptions()
		{
			var sourceBaseOptions = new DeploymentBaseOptions();
			if (!LocalHost)
			{
				sourceBaseOptions.ComputerName = ComputerName;
			}

			if (HasCredentials)
			{
				sourceBaseOptions.UserName = CredentialsProvider.UserName;
				sourceBaseOptions.Password = CredentialsProvider.Password;
			}
			return sourceBaseOptions;
		}

		public bool IsValid(Notification notification)
		{
			if (!LocalHost && string.IsNullOrWhiteSpace(ComputerName))
			{
				notification.AddError(new SemanticValidationError("Neither localhost or computer name is defined for source.", ValidationErrorType.NoSource));
				return true;
			}
			return false;
		}
	}
}
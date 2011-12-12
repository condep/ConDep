using System.Collections.Generic;
using ConDep.WebDeploy.Dsl;
using Microsoft.Web.Deployment;

namespace ConDep.WebDeploy.Dsl.SemanticModel
{
	public class Source : IWebDeployModel
	{
		private Credentials _credentials = new Credentials();

		public string ComputerName { get; set; }
		public bool LocalHost { get; set; }
		public bool HasCredentials
		{
			get { return !string.IsNullOrWhiteSpace(Credentials.UserName); }
		}

		public Credentials Credentials
		{
			get {
				return _credentials;
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
				sourceBaseOptions.UserName = Credentials.UserName;
				sourceBaseOptions.Password = Credentials.Password;
			}
			return sourceBaseOptions;
		}

		public bool IsValid(Notification notification)
		{
			_credentials.IsValid(notification);

			if (!LocalHost && string.IsNullOrWhiteSpace(ComputerName))
			{
				notification.AddError(new SemanticValidationError("Neither localhost or computer name is defined for source.", ValidationErrorType.NoSource));
				return true;
			}
			return false;
		}
	}
}
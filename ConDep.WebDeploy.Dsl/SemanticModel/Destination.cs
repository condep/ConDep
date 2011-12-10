using System.Collections.Generic;
using ConDep.WebDeploy.Dsl;
using Microsoft.Web.Deployment;

namespace ConDep.WebDeploy.Dsl.SemanticModel
{
	public class Destination : IWebDeployModel
	{
		private readonly CredentialsProvider _credentialsProvider = new CredentialsProvider();

		public string ComputerName { get; set; }
		public CredentialsProvider CredentialsProvider
		{
			get { return _credentialsProvider; }
		}

		public DeploymentBaseOptions GetDestinationBaseOptions()
		{
			var destBaseOptions = new DeploymentBaseOptions
			{
				ComputerName = ComputerName,
				UserName = CredentialsProvider.UserName,
				Password = CredentialsProvider.Password
			};
			return destBaseOptions;
		}

		public bool IsValid(Notification notification)
		{
			if (string.IsNullOrWhiteSpace(ComputerName))
			{
				notification.AddError(new SemanticValidationError("No computer name is specified for destination.", ValidationErrorType.NoDestination));
				return true;
			}
			return false;
		}
	}
}
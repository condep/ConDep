using Microsoft.Web.Deployment;

namespace ConDep.Dsl.Core
{
	public class WebDeployDestination : IValidate
	{
		private readonly Credentials _credentials = new Credentials();

		public string ComputerName { get; set; }
		public Credentials Credentials
		{
			get { return _credentials; }
		}

		public DeploymentBaseOptions GetDestinationBaseOptions()
		{
			var destBaseOptions = new DeploymentBaseOptions
			{
				ComputerName = ComputerName,
				UserName = Credentials.UserName,
				Password = Credentials.Password
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
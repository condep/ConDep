using ConDep.WebDeploy.Dsl;
using Microsoft.Web.Deployment;

namespace ConDep.WebDeploy.Dsl.SemanticModel
{
	public class Destination
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
	}
}
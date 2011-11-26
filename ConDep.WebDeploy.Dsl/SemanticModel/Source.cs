using System.Collections.Generic;
using ConDep.WebDeploy.Dsl;
using Microsoft.Web.Deployment;

namespace ConDep.WebDeploy.Dsl.SemanticModel
{
	public class Source
	{
		private readonly List<Provider> _providers = new List<Provider>();
		private CredentialsProvider _credentialsProvider = new CredentialsProvider();

		public string ComputerName { get; set; }
		public bool LocalHost { get; set; }
		public List<Provider> Providers { get { return _providers; } }
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
	}
}
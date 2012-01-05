using ConDep.Dsl.FluentWebDeploy.Operations.WebDeploy.Model;
using ConDep.Dsl.FluentWebDeploy.SemanticModel;

namespace ConDep.Dsl.FluentWebDeploy.Builders
{
	public class CredentialsBuilder
	{
		private readonly Credentials _credentials;

		public CredentialsBuilder(Credentials credentials)
		{
			_credentials = credentials;
		}

		public CredentialsBuilder WithUserName(string userName)
		{
			_credentials.UserName = userName;
			return this;
		}

		public CredentialsBuilder WithPassword(string password)
		{
			_credentials.Password = password;
			return this;
		}
	}
}
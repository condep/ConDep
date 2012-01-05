using ConDep.Dsl.Operations.WebDeploy.Model;

namespace ConDep.Dsl.Builders
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
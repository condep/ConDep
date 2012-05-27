using ConDep.Dsl.Operations.WebDeploy.Model;

namespace ConDep.Dsl.Operations.WebDeploy.Options
{
	public class CredentialsOptions
	{
		private readonly Credentials _credentials;

		public CredentialsOptions(Credentials credentials)
		{
			_credentials = credentials;
		}

		public CredentialsOptions WithUserName(string userName)
		{
			_credentials.UserName = userName;
			return this;
		}

		public CredentialsOptions WithPassword(string password)
		{
			_credentials.Password = password;
			return this;
		}
	}
}
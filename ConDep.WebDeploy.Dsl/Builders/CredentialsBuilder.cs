namespace ConDep.WebDeploy.Dsl
{
	public class CredentialsBuilder
	{
		private readonly CredentialsProvider _credentialsProvider;

		public CredentialsBuilder(CredentialsProvider credentialsProvider)
		{
			_credentialsProvider = credentialsProvider;
		}

		public CredentialsBuilder WithUserName(string userName)
		{
			_credentialsProvider.UserName = userName;
			return this;
		}

		public CredentialsBuilder WithPassword(string password)
		{
			_credentialsProvider.Password = password;
			return this;
		}
	}
}
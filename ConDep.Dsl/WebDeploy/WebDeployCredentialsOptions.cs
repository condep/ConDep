namespace ConDep.Dsl.WebDeploy
{
	public class WebDeployCredentialsOptions
	{
		private readonly Credentials _credentials;

		public WebDeployCredentialsOptions(Credentials credentials)
		{
			_credentials = credentials;
		}

		public WebDeployCredentialsOptions WithUserName(string userName)
		{
			_credentials.UserName = userName;
			return this;
		}

		public WebDeployCredentialsOptions WithPassword(string password)
		{
			_credentials.Password = password;
			return this;
		}
	}
}
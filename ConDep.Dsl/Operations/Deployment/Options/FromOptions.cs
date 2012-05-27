using System;
using ConDep.Dsl.Operations.WebDeploy.Model;

namespace ConDep.Dsl.Operations.WebDeploy.Options
{
	public class FromOptions
	{
		private readonly Source _source;
		private readonly SyncOptions _syncOptions;

		public FromOptions(Source source, SyncOptions syncOptions)
		{
			_source = source;
			_syncOptions = syncOptions;
		}

		public void LocalHost()
		{
		    _source.LocalHost = true;
		}

		public void LocalHost(Action<CredentialsOptions> credentials)
		{
			_source.ComputerName = "127.0.0.1";
			var credBuilder = new CredentialsOptions(_source.Credentials);
			credentials(credBuilder);
		}

		public void Server(string serverName)
		{
			_source.ComputerName = serverName;
		}

		public void Server(string serverName, Action<CredentialsOptions> credentials)
		{
			_source.ComputerName = serverName;

			var credBuilder = new CredentialsOptions(_source.Credentials);
			credentials(credBuilder);
		}

	    public void Package(string packagePath, string encryptionPassword)
	    {
	        _source.PackagePath = packagePath;
	        _source.EncryptionPassword = encryptionPassword;
	    }
	}
}
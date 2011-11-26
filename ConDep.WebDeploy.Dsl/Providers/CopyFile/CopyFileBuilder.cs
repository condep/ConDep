namespace ConDep.WebDeploy.Dsl
{
	public class CopyFileBuilder
	{
		private readonly CopyFileProvider _copyFileProvider;

		public CopyFileBuilder(CopyFileProvider credentials)
		{
			_copyFileProvider = credentials;
		}

		public CopyFileBuilder SetRemotePathTo(string remotePath)
		{
			_copyFileProvider.DestinationPath = remotePath;
			return this;
		}
	}
}
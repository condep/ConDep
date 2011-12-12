namespace ConDep.Dsl.FluentWebDeploy
{
	public class CopyFileBuilder : IProviderBuilder<CopyFileBuilder>
	{
		private readonly CopyFileProvider _copyFileProvider;

		public CopyFileBuilder(CopyFileProvider credentials)
		{
			_copyFileProvider = credentials;
		}

		public IProviderBuilder<CopyFileBuilder> SetRemotePathTo(string remotePath)
		{
			_copyFileProvider.DestinationPath = remotePath;
			return this;
		}
	}
}
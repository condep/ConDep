namespace ConDep.WebDeploy.Dsl
{
	public class CopyDirBuilder
	{
		private readonly CopyDirProvider _copyDirProvider;

		public CopyDirBuilder(CopyDirProvider copyDirProvider)
		{
			_copyDirProvider = copyDirProvider;
		}

		public CopyDirBuilder SetRemotePathTo(string remotePath)
		{
			_copyDirProvider.DestinationPath = remotePath;
			return this;
		}
	}
}
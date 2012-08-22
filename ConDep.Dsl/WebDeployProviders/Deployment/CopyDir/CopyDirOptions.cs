namespace ConDep.Dsl
{
	public class CopyDirOptions
	{
		private readonly CopyDirProvider _copyDirProvider;

		public CopyDirOptions(CopyDirProvider copyDirProvider)
		{
			_copyDirProvider = copyDirProvider;
		}

		public CopyDirOptions DestinationDir(string remotePath)
		{
			_copyDirProvider.DestinationPath = remotePath;
			return this;
		}
	}
}
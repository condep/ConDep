namespace ConDep.Dsl
{
	public class CopyFileOptions 
	{
		private readonly CopyFileProvider _copyFileProvider;

		public CopyFileOptions(CopyFileProvider credentials)
		{
			_copyFileProvider = credentials;
		}

		public CopyFileOptions RenameFileOnDestination(string fileName)
		{
			_copyFileProvider.DestinationPath = fileName;
			return this;
		}
	}
}
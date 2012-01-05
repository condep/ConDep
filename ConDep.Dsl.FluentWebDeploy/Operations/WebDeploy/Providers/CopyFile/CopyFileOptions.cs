using ConDep.Dsl.FluentWebDeploy.Builders;

namespace ConDep.Dsl.FluentWebDeploy
{
	public class CopyFileOptions : IProvideOptions<CopyFileOptions>
	{
		private readonly CopyFileProvider _copyFileProvider;

		public CopyFileOptions(CopyFileProvider credentials)
		{
			_copyFileProvider = credentials;
		}

		public IProvideOptions<CopyFileOptions> RenameFileOnDestination(string fileName)
		{
			_copyFileProvider.DestinationPath = fileName;
			return this;
		}
	}
}
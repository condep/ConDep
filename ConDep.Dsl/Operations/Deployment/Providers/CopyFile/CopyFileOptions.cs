using ConDep.Dsl.Builders;
using ConDep.Dsl.Operations.WebDeploy.Options;

namespace ConDep.Dsl
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
using System;
using ConDep.Dsl.Builders;
using ConDep.Dsl.Operations.WebDeploy.Options;

namespace ConDep.Dsl
{
	public static class CopyFileExtension
	{
		public static void CopyFile(this ProviderCollection providerCollection, string path, Action<CopyFileOptions> options)
		{
			var copyFileProvider = new CopyFileProvider(path);
			options(new CopyFileOptions(copyFileProvider));
			providerCollection.AddProvider(copyFileProvider);
		}
	}
}
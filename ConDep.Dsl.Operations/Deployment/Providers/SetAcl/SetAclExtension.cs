using System;
using ConDep.Dsl.Core;

namespace ConDep.Dsl
{
	public static class SetAclExtension
	{
        public static void SetAcl(this ProviderOptions providerOptions, string path, Action<SetAclOptions> options)
		{
			var setAclProvider = new SetAclProvider(path);
			options(new SetAclOptions(setAclProvider));
			providerOptions.AddProvider(setAclProvider);
		}

        public static void SetAcl(this IProvideForDeployment providerCollection, string path, Action<SetAclOptions> options)
        {
            var setAclProvider = new SetAclProvider(path);
            options(new SetAclOptions(setAclProvider));
            providerCollection.AddProvider(setAclProvider);
        }
    }
}
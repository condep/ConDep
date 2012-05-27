using System;
using ConDep.Dsl.Builders;
using ConDep.Dsl.Operations.WebDeploy.Options;

namespace ConDep.Dsl
{
	public static class SetAclExtension
	{
        public static void SetAcl(this ProviderCollection providerCollection, string path, Action<SetAclOptions> options)
		{
			var setAclProvider = new SetAclProvider(path);
			options(new SetAclOptions(setAclProvider));
			providerCollection.AddProvider(setAclProvider);
		}

        public static void SetAcl(this IProvideForServer providerCollection, string path, Action<SetAclOptions> options)
        {
            var setAclProvider = new SetAclProvider(path);
            options(new SetAclOptions(setAclProvider));
            providerCollection.AddProvider(setAclProvider);
        }
    }
}
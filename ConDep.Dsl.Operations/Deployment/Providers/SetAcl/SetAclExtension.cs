using System;
using ConDep.Dsl.Core;

namespace ConDep.Dsl
{
	public static class SetAclExtension
	{
        public static void SetAcl(this IProvideForInfrastructure providerOptions, string path, Action<SetAclOptions> aclOptions)
        {
            var options = (InfrastructureProviderOptions) providerOptions;
			var setAclProvider = new SetAclProvider(path);
			aclOptions(new SetAclOptions(setAclProvider));
            options.WebDeploySetup.ConfigureProvider(setAclProvider);
		}
    }
}
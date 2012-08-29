using System;
using ConDep.Dsl.Core;

namespace ConDep.Dsl
{
	public static class SetAclExtension
	{
        //Todo: Check out how WebDeploy uses the SetAcl provider (Access, Resource, User and it's default values)
        public static void SetAcl(this ProvideForInfrastructure providerOptions, string path, Action<SetAclOptions> aclOptions)
        {
            //var options = (InfrastructureProviderOptions) providerOptions;
			var setAclProvider = new SetAclProvider(path);
			aclOptions(new SetAclOptions(setAclProvider));
            ((IProvideOptions)providerOptions).AddProviderAction(setAclProvider);
            //options.WebDeploySetup.ConfigureProvider(setAclProvider);
		}
    }
}
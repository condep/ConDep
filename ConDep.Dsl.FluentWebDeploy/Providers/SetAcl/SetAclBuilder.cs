using System.Security.AccessControl;
using ConDep.Dsl.FluentWebDeploy.Builders;

namespace ConDep.Dsl.FluentWebDeploy
{
	public class SetAclBuilder : IProviderBuilder<SetAclBuilder>
	{
		private readonly SetAclProvider _setAclProvider;

		public SetAclBuilder(SetAclProvider setAclProvider)
		{
			_setAclProvider = setAclProvider;
		}

		public void Permissions(FileSystemRights accessRights, string userName)
		{
			_setAclProvider.Permissions = accessRights;
			_setAclProvider.User = userName;
		}
	}
}
using System.Security.AccessControl;

namespace ConDep.WebDeploy.Dsl
{
	public class SetAclBuilder
	{
		private readonly SetAclProvider _setAclProvider;

		public SetAclBuilder(SetAclProvider setAclProvider)
		{
			_setAclProvider = setAclProvider;
		}

		public SetAclBuilder Permissions(FileSystemRights accessRights, string userName)
		{
			_setAclProvider.Permissions = accessRights;
			_setAclProvider.User = userName;
			return this;
		}
	}
}
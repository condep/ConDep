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

		public SetAclBuilder Permissions(FileSystemRights accessRights)
		{
			_setAclProvider.Permissions = accessRights;
			return this;
		}

		public SetAclBuilder User(string userName)
		{
			_setAclProvider.User = userName;
			return this;
		}
	}
}
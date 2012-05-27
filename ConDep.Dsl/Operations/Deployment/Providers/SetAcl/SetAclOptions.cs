using System.Security.AccessControl;
using ConDep.Dsl.Builders;
using ConDep.Dsl.Operations.WebDeploy.Options;

namespace ConDep.Dsl
{
	public class SetAclOptions : IProvideOptions<SetAclOptions>
	{
		private readonly SetAclProvider _setAclProvider;

		public SetAclOptions(SetAclProvider setAclProvider)
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
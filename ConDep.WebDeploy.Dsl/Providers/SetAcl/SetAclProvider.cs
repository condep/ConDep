using System.Security.AccessControl;
using ConDep.WebDeploy.Dsl.SemanticModel;
using Microsoft.Web.Deployment;

namespace ConDep.WebDeploy.Dsl
{
	public class SetAclProvider : Provider
	{
		public SetAclProvider(string destinationPath)
		{
			DestinationPath = destinationPath;
			Name = "setAcl";
		}

		public FileSystemRights Permissions { get; set; }
		public string User { get; set; }

		public override DeploymentProviderOptions GetWebDeployDestinationProviderOptions()
		{
			var destProviderOptions = new DeploymentProviderOptions(Name) { Path = DestinationPath };

			DeploymentProviderSetting userSetting;
			if (destProviderOptions.ProviderSettings.TryGetValue("setAclUser", out userSetting))
			{
				userSetting.Value = User;
			}

			DeploymentProviderSetting permissionSetting;
			if (destProviderOptions.ProviderSettings.TryGetValue("setAclAccess", out permissionSetting))
			{
				permissionSetting.Value = Permissions.ToString();
			}
			return destProviderOptions;
		}

		public override DeploymentObject GetWebDeploySourceObject(DeploymentBaseOptions sourceBaseOptions)
		{
			return DeploymentManager.CreateObject(Name, "", sourceBaseOptions);
		}

		public override bool IsValid(Notification notification)
		{
			throw new System.NotImplementedException();
		}
	}
}
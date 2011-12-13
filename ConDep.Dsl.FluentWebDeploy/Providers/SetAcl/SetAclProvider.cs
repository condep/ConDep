using System.Security.AccessControl;
using ConDep.Dsl.FluentWebDeploy.SemanticModel;
using Microsoft.Web.Deployment;

namespace ConDep.Dsl.FluentWebDeploy
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

		public override DeploymentProviderOptions GetWebDeployDestinationObject()
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
			return !string.IsNullOrWhiteSpace(DestinationPath) && !string.IsNullOrWhiteSpace(User) && !string.IsNullOrWhiteSpace(Permissions.ToString());
		}
	}
}
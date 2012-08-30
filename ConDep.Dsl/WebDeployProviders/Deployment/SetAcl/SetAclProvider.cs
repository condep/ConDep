using System.Security.AccessControl;
using ConDep.Dsl;
using Microsoft.Web.Deployment;
using DeploymentProviderOptions = Microsoft.Web.Deployment.DeploymentProviderOptions;

namespace ConDep.Dsl
{
	public class SetAclProvider : WebDeployProviderBase
	{
		public SetAclProvider(string destinationPath)
		{
			DestinationPath = destinationPath;
		}

		public FileSystemRights Permissions { get; set; }
		public string User { get; set; }

		public override string Name
		{
			get { return "setAcl"; }
		}

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
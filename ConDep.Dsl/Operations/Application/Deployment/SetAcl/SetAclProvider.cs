using System.Security.AccessControl;
using ConDep.Dsl.SemanticModel;
using ConDep.Dsl.SemanticModel.WebDeploy;
using Microsoft.Web.Deployment;

namespace ConDep.Dsl.Operations.Application.Deployment.SetAcl
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

	    public override DeploymentProviderOptions GetWebDeploySourceProviderOptions()
	    {
            return new DeploymentProviderOptions(Name) { Path = SourcePath };
        }

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

		public override bool IsValid(Notification notification)
		{
			return !string.IsNullOrWhiteSpace(DestinationPath) && !string.IsNullOrWhiteSpace(User) && !string.IsNullOrWhiteSpace(Permissions.ToString());
		}
	}
}
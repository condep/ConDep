using System.Collections.Generic;
using Microsoft.Web.Deployment;

namespace ConDep.WebDeploy.Dsl.SemanticModel
{
	public class BasicProvider : Provider
	{
		private Dictionary<string, string> _providerSettings = new Dictionary<string,string>();

		public override DeploymentProviderOptions GetWebDeployDestinationProviderOptions()
		{
			var destProviderOptions = new DeploymentProviderOptions(Name) { Path = DestinationPath };

			foreach(var setting in _providerSettings)
			{
				DeploymentProviderSetting userSetting;
				if (destProviderOptions.ProviderSettings.TryGetValue(setting.Key, out userSetting))
				{
					userSetting.Value = setting.Value;
				}
			}

			return destProviderOptions;
		}

		public override DeploymentObject GetWebDeploySourceObject(DeploymentBaseOptions sourceBaseOptions)
		{
			return DeploymentManager.CreateObject(Name, SourcePath, sourceBaseOptions);
		}

		public override bool IsValid(Notification notification)
		{
			throw new System.NotImplementedException();
		}

		public Dictionary<string, string> ProviderSettings
		{
			get {
				return _providerSettings;
			}
		}
	}
}
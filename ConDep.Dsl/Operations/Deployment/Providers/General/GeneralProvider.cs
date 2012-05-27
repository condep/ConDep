using System.Collections.Generic;
using ConDep.Dsl.Operations.WebDeploy.Model;
using Microsoft.Web.Deployment;

namespace ConDep.Dsl
{
	public class GeneralProvider : ExistingServerProvider
	{
		private readonly Dictionary<string, string> _providerSettings = new Dictionary<string,string>();
		private readonly string _name;

		public GeneralProvider(string providername)
		{
			_name = providername;
		}

		public override string Name
		{
			get { return _name; }
		}

		public override DeploymentProviderOptions GetWebDeployDestinationObject()
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
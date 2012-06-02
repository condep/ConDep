using System;
using ConDep.Dsl.Operations.WebDeploy.Model;
using Microsoft.Web.Deployment;
using System.Linq;

namespace ConDep.Dsl
{
	public class CertficiateProvider : WebDeployProvider
	{
		private const string NAME = "cert";

		public CertficiateProvider(string thumbprint)
		{
			SourcePath = thumbprint;
		}

		public override string Name
		{
			get { return NAME; }
		}

		public override DeploymentProviderOptions GetWebDeployDestinationObject()
		{
			return new DeploymentProviderOptions(DeploymentWellKnownProvider.Auto);
		}

		public override DeploymentObject GetWebDeploySourceObject(DeploymentBaseOptions sourceBaseOptions)
		{
			var obj = DeploymentManager.CreateObject(Name, SourcePath, sourceBaseOptions);
			return obj;
		}

		public override bool IsValid(Notification notification)
		{
			return !string.IsNullOrWhiteSpace(SourcePath);
		}
	}
}
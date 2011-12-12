using System;
using ConDep.WebDeploy.Dsl.Builders;
using ConDep.WebDeploy.Dsl.Deployment;
using ConDep.WebDeploy.Dsl.SemanticModel;

namespace ConDep.WebDeploy.Dsl
{
	public abstract class WebDeployOperation
	{
		private readonly WebDeployDefinition _definition;
		private readonly IWebDeploy _webDeployer;

		protected WebDeployOperation()
		{
			_definition = new WebDeployDefinition();
			_webDeployer = new Deployment.WebDeploy();
			HookUpDeployEvents();
		}

		//protected WebDeployOperation(WebDeployDefinition definition, IWebDeploy webDeployer)
		//{
		//   _definition = definition;
		//   _webDeployer = webDeployer;
		//   HookUpDeployEvents();
		//}

		private void HookUpDeployEvents()
		{
			_webDeployer.Output += OnWebDeployMessage;
			_webDeployer.OutputError += OnWebDeployErrorMessage;
		}


		public void Sync(Action<SyncBuilder> action)
		{
			action(new SyncBuilder(_definition));
			_webDeployer.Deploy(_definition);
		}

		public void Delete(Action<DeleteBuilder> action)
		{
			var definition = new WebDeployDefinition();
			action(new DeleteBuilder(new WebDeployDefinition()));

			var webDeploy = new Deployment.WebDeploy();
			webDeploy.Delete(definition);
		}

		protected abstract void OnWebDeployMessage(object sender, WebDeployMessageEventArgs e);
		protected abstract void OnWebDeployErrorMessage(object sender, WebDeployMessageEventArgs e);

		//public void Package(Action<IPackage> action)
		//{
		//   action(new SyncBuilder(new WebDeployDefinition()));
		//}

		//public void Archive(Action<IArchive> action)
		//{
		//   action(new SyncBuilder(new WebDeployDefinition()));
		//}
	}
}
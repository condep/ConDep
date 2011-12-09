using System;
using ConDep.WebDeploy.Dsl.Builders;
using ConDep.WebDeploy.Dsl.SemanticModel;

namespace ConDep.WebDeploy.Dsl
{
	public abstract class WebDeployOperation
	{
		private readonly WebDeployDefinition _definition;
		private readonly IWebDeploy _webDeployer;

		protected WebDeployOperation(WebDeployDefinition definition, IWebDeploy webDeployer)
		{
			_definition = definition;
			_webDeployer = webDeployer;
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

			var webDeploy = new WebDeploy();
			webDeploy.Delete(definition);
		}

		public abstract void OnWebDeployMessage(object sender, WebDeployMessegaEventArgs e);

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
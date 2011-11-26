using System;
using ConDep.WebDeploy.Dsl.Builders;
using ConDep.WebDeploy.Dsl.SemanticModel;

namespace ConDep.WebDeploy.Dsl
{
	public class WebDeployOperation 
	{
		public void Sync(Action<SyncBuilder> action)
		{
			var definition = new WebDeployDefinition();
			action(new SyncBuilder(definition));

			var webDeploy = new WebDeploy(definition);
			webDeploy.Deploy();
		}

		public void Delete(Action<DeleteBuilder> action)
		{
			var definition = new WebDeployDefinition();
			action(new DeleteBuilder(new WebDeployDefinition()));

			var webDeploy = new WebDeploy(definition);
			webDeploy.Delete();
		}

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
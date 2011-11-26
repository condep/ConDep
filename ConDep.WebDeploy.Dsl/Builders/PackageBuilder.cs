using ConDep.WebDeploy.Dsl.SemanticModel;

namespace ConDep.WebDeploy.Dsl.Builders
{
	public class PackageBuilder
	{
		private readonly WebDeployDefinition _webDeployDefinition;

		public PackageBuilder(WebDeployDefinition webDeployDefinition)
		{
			_webDeployDefinition = webDeployDefinition;
		}
	}
}
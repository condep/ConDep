using ConDep.WebDeploy.Dsl.Builders;
using ConDep.WebDeploy.Dsl.SemanticModel;

namespace ConDep.WebDeploy.Dsl
{
	public class ConsoleBase
	{
		public static SyncBuilder Sync() 
		{
			return new SyncBuilder(new WebDeployDefinition());
		}
	}
}
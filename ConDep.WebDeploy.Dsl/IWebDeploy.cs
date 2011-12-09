using ConDep.WebDeploy.Dsl.SemanticModel;

namespace ConDep.WebDeploy.Dsl
{
	public interface IWebDeploy
	{
		void Deploy(WebDeployDefinition webDeployDefinition);
		void Delete(WebDeployDefinition webDeployDefinition);
		//WebDeployDefinition Definition { get; set; }
	}
}
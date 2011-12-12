using ConDep.Dsl.FluentWebDeploy.SemanticModel;

namespace ConDep.Dsl.FluentWebDeploy.Builders
{
	public class ConfigurationBuilder
	{
		private readonly Configuration _configuration;

		public ConfigurationBuilder(Configuration configuration)
		{
			_configuration = configuration;
		}
		
		public ConfigurationBuilder AutoDeployAgent()
		{
			_configuration.AutoDeployAgent = true;
			return this;
		}
	}
}
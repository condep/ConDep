using ConDep.Dsl.FluentWebDeploy.Operations.WebDeploy.Model;
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
		
		public ConfigurationBuilder DoNotAutoDeployAgent()
		{
			_configuration.DoNotAutoDeployAgent = true;
			return this;
		}
	}
}
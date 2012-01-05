using ConDep.Dsl.Operations.WebDeploy.Model;

namespace ConDep.Dsl.Builders
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
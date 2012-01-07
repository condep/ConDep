using ConDep.Dsl.Operations.WebDeploy.Model;

namespace ConDep.Dsl.Operations.WebDeploy.Options
{
	public class ConfigurationOptions
	{
		private readonly Configuration _configuration;

		public ConfigurationOptions(Configuration configuration)
		{
			_configuration = configuration;
		}
		
		public ConfigurationOptions DoNotAutoDeployAgent()
		{
			_configuration.DoNotAutoDeployAgent = true;
			return this;
		}
	}
}
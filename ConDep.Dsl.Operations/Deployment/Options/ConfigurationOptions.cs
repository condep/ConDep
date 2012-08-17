using ConDep.Dsl.Core;

namespace ConDep.Dsl.Operations.Deployment.Options
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
			_configuration.DoNotAutoDeployAgent = false;
			return this;
		}

	    public ConfigurationOptions UseWhatIf()
	    {
	        _configuration.UseWhatIf = true;
	        return this;
	    }
	}
}
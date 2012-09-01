namespace ConDep.Dsl.WebDeploy
{
	public class WebDeployConfigurationOptions
	{
		private readonly Configuration _configuration;

		public WebDeployConfigurationOptions(Configuration configuration)
		{
			_configuration = configuration;
		}
		
		public WebDeployConfigurationOptions DoNotAutoDeployAgent()
		{
			_configuration.DoNotAutoDeployAgent = false;
			return this;
		}

	    public WebDeployConfigurationOptions UseWhatIf()
	    {
	        _configuration.UseWhatIf = true;
	        return this;
	    }
	}
}
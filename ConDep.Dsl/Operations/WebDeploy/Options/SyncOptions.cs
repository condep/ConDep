using System;
using ConDep.Dsl.Operations.WebDeploy.Model;

namespace ConDep.Dsl.Operations.WebDeploy.Options
{
	public class SyncOptions
	{
		private readonly WebDeployDefinition _webDeployDefinition;
	    private ProviderCollection _providerCollection;
	    private FromOptions _fromOptions;
	    private ToOptions _toOptions;

	    public SyncOptions(WebDeployDefinition webDeployDefinition)
		{
			_webDeployDefinition = webDeployDefinition;
		}

		public void WithConfiguration(Action<ConfigurationOptions> action)
		{
			var configBuilder = new ConfigurationOptions(_webDeployDefinition.Configuration);
			action(configBuilder);
		}

	    public ProviderCollection Using
	    {
	        get { return _providerCollection ?? (_providerCollection = new ProviderCollection(_webDeployDefinition.Providers)); }
	    }

		public FromOptions From
		{
			get { return _fromOptions ?? (_fromOptions = new FromOptions(_webDeployDefinition.Source, this)); }
		}

		public ToOptions To
		{
			get { return _toOptions ?? (_toOptions = new ToOptions(_webDeployDefinition)); }
		}
	}
}
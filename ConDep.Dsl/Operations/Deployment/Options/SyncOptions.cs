using System;
using System.Collections.Generic;
using ConDep.Dsl.Builders;
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

    public class ServerOptions : IProvideForServer
    {
        private WebDeployDefinition _webDeployDefinition;
        private readonly List<IProvide> _providers;
        private IisOptions _iisOptions;

        public ServerOptions(WebDeployDefinition webDeployDefinition, List<IProvide> providers)
        {
            _webDeployDefinition = webDeployDefinition;
            _providers = providers;
        }

        public IisOptions IIS
        {
            get { return _iisOptions ?? (_iisOptions = new IisOptions()); }
        }

        public void AddProvider(IProvide provider)
        {
            _providers.Add(provider);

            if (provider is CompositeProvider)
            {
                ((CompositeProvider)provider).Configure();
            }
        }
    }
}
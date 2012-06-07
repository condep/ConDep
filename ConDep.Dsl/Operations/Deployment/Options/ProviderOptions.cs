using System;
using System.Collections.Generic;
using ConDep.Dsl.Builders;
using ConDep.Dsl.Operations.WebDeploy.Model;

namespace ConDep.Dsl.Operations.WebDeploy.Options
{
    public class ProviderOptions : IProvideForExistingIisServer, IProvideForCustomIisDefinition, IProvideForInfrastructureIis
    {
        private readonly List<IProvide> _providers;

        public ProviderOptions(List<IProvide> providers)
        {
            _providers = providers;
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

    public class DeploymentProviderOptions : ProviderOptions, IProvideForDeployment, IProvideForInfrastructure
    {
        private readonly WebDeployDefinition _webDeployDefinition;
        private IisOptions _iisOptions;
        private WindowsOptions _windowsOptions;

        public DeploymentProviderOptions(WebDeployDefinition webDeployDefinition) : base(webDeployDefinition.Providers)
        {
            _webDeployDefinition = webDeployDefinition;
        }

        public IisOptions IIS
        {
            get { return _iisOptions ?? (_iisOptions = new IisOptions(_webDeployDefinition)); }
        }

        public WindowsOptions Windows
        {
            get { return _windowsOptions ?? (_windowsOptions = new WindowsOptions(_webDeployDefinition)); }
        }
    }
}
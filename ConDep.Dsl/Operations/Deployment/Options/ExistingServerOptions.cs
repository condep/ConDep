using ConDep.Dsl.Operations.WebDeploy.Model;
using ConDep.Dsl.Operations.WebDeploy.Options;

namespace ConDep.Dsl.Builders
{
    public class ExistingServerOptions
    {
        private IProvideForExistingIisServer _providerCollection;
        private WebDeployDefinition _webDeployDefinition;

        public ExistingServerOptions(WebDeployDefinition webDeployDefinition)
		{
			_webDeployDefinition = webDeployDefinition;
		}

        public IProvideForExistingIisServer Using
        {
            get { return _providerCollection ?? (_providerCollection = new ProviderCollection(_webDeployDefinition.Providers)); }
        }
    }
}   
using System;
using ConDep.Dsl.Core;

namespace ConDep.Dsl
{
    public class WebSiteInfrastructureOptions : ProviderOptions, IProvideForCustomWebSite
    {
        private readonly WebSiteInfrastructureProvider _webSiteInfrastructureProvider;

        //Todo: Somehow this should probably get DeployentServer injected
        public WebSiteInfrastructureOptions(WebSiteInfrastructureProvider webSiteInfrastructureProvider) : base(webSiteInfrastructureProvider.ChildProviders)
        {
            _webSiteInfrastructureProvider = webSiteInfrastructureProvider;
        }

        //public void HttpBinding(int port)
        //{
        //    var binding = new IisBinding(BindingType.http, port);
        //    _webSiteInfrastructureProvider.Bindings.Add(binding);
        //}

        //public void HttpsBinding(int port, string certificateCommonName)
        //{
        //    var binding = new IisBinding(BindingType.https, port)
        //                      {
        //                          CertificateCommonName = certificateCommonName
        //                      };
        //    _webSiteInfrastructureProvider.Bindings.Add(binding);
        //}

        //public void HttpBinding(int port, Action<BindingOptions> bindingOptions)
        //{
        //    var binding = new IisBinding(BindingType.http, port);
        //    _webSiteInfrastructureProvider.Bindings.Add(binding);
        //    bindingOptions(new BindingOptions(binding));
        //}

        //public void HttpsBinding(int port, string certificateCommonName, Action<BindingOptions> bindingOptions)
        //{
        //    var binding = new IisBinding(BindingType.https, port)
        //                      {
        //                          CertificateCommonName = certificateCommonName
        //                      };
        //    _webSiteInfrastructureProvider.Bindings.Add(binding);
        //    bindingOptions(new BindingOptions(binding));
        //}

        public void ApplicationPool(string appPoolName)
        {
            _webSiteInfrastructureProvider.ApplicationPool.Name = appPoolName;
        }

        public void ApplicationPool(string appPoolName, Action<AppPoolInfrastructureOptions> appPoolOptions)
        {
            _webSiteInfrastructureProvider.ApplicationPool.Name = appPoolName;
            appPoolOptions(new AppPoolInfrastructureOptions(_webSiteInfrastructureProvider));
        }

        //public void AddProvider(IProvide provider)
        //{
        //    _webSiteInfrastructureProvider.ChildProviders.Add(provider);

        //    if (provider is CompositeProvider)
        //    {
        //        ((CompositeProvider)provider).Configure(null);
        //    }
        //}

        public string WebSiteName
        {
            get { return _webSiteInfrastructureProvider.WebSiteName; }
        }
    }
}
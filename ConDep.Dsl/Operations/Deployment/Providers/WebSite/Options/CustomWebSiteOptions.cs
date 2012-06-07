using System;
using ConDep.Dsl.Operations.WebDeploy.Model;
using ConDep.Dsl.Operations.WebDeploy.Options;

namespace ConDep.Dsl
{
    public class CustomWebSiteOptions : IProvideForCustomWebSite
    {
        private readonly CustomWebSiteProvider _customWebSiteProvider;

        public CustomWebSiteOptions(CustomWebSiteProvider customWebSiteProvider)
        {
            _customWebSiteProvider = customWebSiteProvider;
        }

        //public void HttpBinding(int port)
        //{
        //    var binding = new IisBinding(BindingType.http, port);
        //    _customWebSiteProvider.Bindings.Add(binding);
        //}

        //public void HttpsBinding(int port, string certificateCommonName)
        //{
        //    var binding = new IisBinding(BindingType.https, port)
        //                      {
        //                          CertificateCommonName = certificateCommonName
        //                      };
        //    _customWebSiteProvider.Bindings.Add(binding);
        //}

        //public void HttpBinding(int port, Action<BindingOptions> bindingOptions)
        //{
        //    var binding = new IisBinding(BindingType.http, port);
        //    _customWebSiteProvider.Bindings.Add(binding);
        //    bindingOptions(new BindingOptions(binding));
        //}

        //public void HttpsBinding(int port, string certificateCommonName, Action<BindingOptions> bindingOptions)
        //{
        //    var binding = new IisBinding(BindingType.https, port)
        //                      {
        //                          CertificateCommonName = certificateCommonName
        //                      };
        //    _customWebSiteProvider.Bindings.Add(binding);
        //    bindingOptions(new BindingOptions(binding));
        //}

        public void ApplicationPool(string appPoolName)
        {
            _customWebSiteProvider.ApplicationPool.Name = appPoolName;
        }

        public void ApplicationPool(string appPoolName, Action<AppPoolOptions> appPoolOptions)
        {
            _customWebSiteProvider.ApplicationPool.Name = appPoolName;
            appPoolOptions(new AppPoolOptions(_customWebSiteProvider));
        }

        public void AddProvider(IProvide provider)
        {
            _customWebSiteProvider.ChildProviders.Add(provider);

            if (provider is CompositeProvider)
            {
                ((CompositeProvider)provider).Configure();
            }
        }

        public string WebSiteName
        {
            get { return _customWebSiteProvider.WebSiteName; }
        }
    }
}
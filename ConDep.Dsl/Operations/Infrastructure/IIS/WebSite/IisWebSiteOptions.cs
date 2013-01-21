using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using ConDep.Dsl.Builders;

namespace ConDep.Dsl.Operations.Infrastructure.IIS.WebSite
{
    public class IisWebSiteOptions : IOfferIisWebSiteOptions
    {
        private readonly IisWebSiteOptionsValues _values = new IisWebSiteOptionsValues();

        public IOfferIisWebSiteOptions PhysicalPath(string path)
        {
            _values.PhysicalPath = path;
            return this;
        }

        public IOfferIisWebSiteOptions AddHttpBinding(Action<IOfferBindingOptions> httpBindingOptions)
        {
            var options = new BindingOptions();
            httpBindingOptions(options);
            _values.HttpBindings.Add(options.Values);
            return this;
        }

        public IOfferIisWebSiteOptions AddHttpsBinding(X509FindType findType, string findName, Action<IOfferBindingOptions> bindingOptions)
        {
            var options = new BindingOptions();
            bindingOptions(options);

            var httpsOptions = new BindingOptions.SslBindingOptionsValues
                                   {
                                       FindType = findType, 
                                       FindName = findName, 
                                       BindingOptions = options.Values, 
                                       CertLocation = CertLocation.Store
                                   };

            _values.HttpsBindings.Add(httpsOptions);
            return this;
        }

        public IOfferIisWebSiteOptions ApplicationPool(string appPoolName)
        {
            _values.AppPool = appPoolName;
            return this;
        }

        public IOfferIisWebSiteOptions WebApp(string name)
        {
            _values.AddWebApp(name);
            return this;
        }

        public IOfferIisWebSiteOptions WebApp(string name, Action<IOfferIisWebAppOptions> options)
        {
            _values.AddWebApp(name, options);
            return this;
        }

        public IisWebSiteOptionsValues Values { get { return _values; } }

        public class IisWebSiteOptionsValues
        {
            private readonly IList<BindingOptions.BindingOptionsValues> _httpBindingValues = new List<BindingOptions.BindingOptionsValues>();
            private readonly IList<BindingOptions.SslBindingOptionsValues> _httpsBindingValues = new List<BindingOptions.SslBindingOptionsValues>();
            private readonly List<Tuple<string, Action<IOfferIisWebAppOptions>>> _webApps = new List<Tuple<string, Action<IOfferIisWebAppOptions>>>();

            public IList<BindingOptions.BindingOptionsValues> HttpBindings { get { return _httpBindingValues; } }
            public IList<BindingOptions.SslBindingOptionsValues> HttpsBindings { get { return _httpsBindingValues; } }

            public string PhysicalPath { get; set; }
            public string AppPool { get; set; }
            public IEnumerable<Tuple<string, Action<IOfferIisWebAppOptions>>> WebApps { get { return _webApps; } }

            public void AddWebApp(string name, Action<IOfferIisWebAppOptions> options = null)
            {
                _webApps.Add(new Tuple<string, Action<IOfferIisWebAppOptions>>(name, options));                
            }
        }
    }
}
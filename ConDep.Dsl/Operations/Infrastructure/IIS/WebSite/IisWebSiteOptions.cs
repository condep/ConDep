using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

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

        public IOfferIisWebSiteOptions AddHttpsBinding(string filePath, Action<IOfferBindingOptions> bindingOptions)
        {
            var options = new BindingOptions();
            bindingOptions(options);

            var httpsOptions = new BindingOptions.SslBindingOptionsValues
            {
                FilePath = filePath,
                BindingOptions = options.Values,
                CertLocation = CertLocation.File
            };

            _values.HttpsBindings.Add(httpsOptions);
            return this;
        }

        public IOfferIisWebSiteOptions AddHttpsBinding(string filePath, string privateKeyPassword, Action<IOfferBindingOptions> bindingOptions)
        {
            var options = new BindingOptions();
            bindingOptions(options);

            var httpsOptions = new BindingOptions.SslBindingOptionsValues
            {
                FilePath = filePath,
                PrivateKeyPassword = privateKeyPassword,
                BindingOptions = options.Values,
                CertLocation = CertLocation.File
            };

            _values.HttpsBindings.Add(httpsOptions);
            return this;
        }

        public IOfferIisWebSiteOptions ApplicationPool(string appPoolName)
        {
            _values.AppPool = appPoolName;
            return this;
        }

        public IisWebSiteOptionsValues Values { get { return _values; } }

        public class IisWebSiteOptionsValues
        {
            private readonly IList<BindingOptions.BindingOptionsValues> _httpBindingValues = new List<BindingOptions.BindingOptionsValues>();
            private readonly IList<BindingOptions.SslBindingOptionsValues> _httpsBindingValues = new List<BindingOptions.SslBindingOptionsValues>();

            public IList<BindingOptions.BindingOptionsValues> HttpBindings { get { return _httpBindingValues; } }
            public IList<BindingOptions.SslBindingOptionsValues> HttpsBindings { get { return _httpsBindingValues; } }

            public string PhysicalPath { get; set; }
            public string AppPool { get; set; }
        }
    }
}
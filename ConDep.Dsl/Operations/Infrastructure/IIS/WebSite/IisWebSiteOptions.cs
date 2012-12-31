using System;

namespace ConDep.Dsl.Operations.Infrastructure.IIS.WebSite
{
    public interface IOfferIisWebSiteOptions
    {
        IOfferIisWebSiteOptions HttpBinding(Action<IOfferHttpBindingOptions> httpBindingOptions);
        IOfferIisWebSiteOptions HttpsBinding(Action<IOfferHttpsBindingOptions> httpBindingOptions);
    }

    public interface IOfferHttpBindingOptions
    {
        IOfferHttpBindingOptions Ip(string ip);
        IOfferHttpBindingOptions Port(int port);
        IOfferHttpBindingOptions HostName(string hostName);
    }

    public interface IOfferHttpsBindingOptions
    {
        IOfferHttpsBindingOptions Ip(string ip);
        IOfferHttpsBindingOptions Port(int port);
        IOfferHttpsBindingOptions HostName(string hostName);
        /// <summary>
        /// Bind existing certificate to web site. To deploy certificate, use the certificate operation for your application artifact.
        /// </summary>
        /// <param name="commonName">The certificate common name to look for in certificate store. Supports wildcard search.</param>
        /// <returns></returns>
        IOfferHttpsBindingOptions Certificate(string commonName);
    }

    public class IisWebSiteOptions : IOfferIisWebSiteOptions
    {
        private readonly HttpBindingOptions _httpBindingOptions = new HttpBindingOptions();
        private readonly HttpsBindingOptions _httpsBindingOptions = new HttpsBindingOptions();
        private readonly IisWebSiteOptionsValues _values = new IisWebSiteOptionsValues();

        public IOfferIisWebSiteOptions HttpBinding(Action<IOfferHttpBindingOptions> httpBindingOptions)
        {
            httpBindingOptions(_httpBindingOptions);
            _values.HttpBindingValues = _httpBindingOptions.Values;
            return this;
        }

        public IOfferIisWebSiteOptions HttpsBinding(Action<IOfferHttpsBindingOptions> httpsBindingOptions)
        {
            httpsBindingOptions(_httpsBindingOptions);
            _values.HttpsBindingValues = _httpsBindingOptions.Values;
            return this;
        }

        public IisWebSiteOptionsValues Values { get { return _values; } }

        public class IisWebSiteOptionsValues
        {
            public HttpBindingOptions.HttpBindingOptionsValues HttpBindingValues { get; set; }
            public HttpsBindingOptions.HttpsBindingOptionsValues HttpsBindingValues { get; set; }
        }
    }

    public class HttpsBindingOptions : IOfferHttpsBindingOptions
    {
        private readonly HttpsBindingOptionsValues _values = new HttpsBindingOptionsValues();

        public IOfferHttpsBindingOptions Ip(string ip)
        {
            _values.Ip = ip;
            return this;
        }

        public IOfferHttpsBindingOptions Port(int port)
        {
            _values.Port = port;
            return this;
        }

        public IOfferHttpsBindingOptions HostName(string hostName)
        {
            _values.HostName = hostName;
            return this;
        }

        public IOfferHttpsBindingOptions Certificate(string certName)
        {
            _values.Certificate = certName;
            return this;
        }

        public HttpsBindingOptionsValues Values { get { return _values; } }

        public class HttpsBindingOptionsValues
        {
            public int Port { get; set; }
            public string Ip { get; set; }
            public string HostName { get; set; }
            public string Certificate { get; set; }
        }
    }

    public class HttpBindingOptions : IOfferHttpBindingOptions
    {
        private readonly HttpBindingOptionsValues _values = new HttpBindingOptionsValues();

        public IOfferHttpBindingOptions Ip(string ip)
        {
            _values.Ip = ip;
            return this;
        }

        public IOfferHttpBindingOptions Port(int port)
        {
            _values.Port = port;
            return this;
        }

        public IOfferHttpBindingOptions HostName(string hostName)
        {
            _values.HostName = hostName;
            return this;
        }

        public HttpBindingOptionsValues Values { get { return _values; } }

        public class HttpBindingOptionsValues
        {
            public int Port { get; set; }
            public string Ip { get; set; }
            public string HostName { get; set; }
        }
    }
}
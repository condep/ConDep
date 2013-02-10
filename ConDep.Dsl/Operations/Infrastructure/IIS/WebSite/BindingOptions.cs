using System;
using System.Security.Cryptography.X509Certificates;

namespace ConDep.Dsl.Operations.Infrastructure.IIS.WebSite
{
    public class BindingOptions : IOfferBindingOptions
    {
        private readonly BindingOptionsValues _values = new BindingOptionsValues();

        public IOfferBindingOptions Ip(string ip)
        {
            _values.Ip = ip;
            return this;
        }

        public IOfferBindingOptions Port(int port)
        {
            _values.Port = port;
            return this;
        }

        public IOfferBindingOptions HostName(string hostName)
        {
            _values.HostName = hostName;
            return this;
        }

        public BindingOptionsValues Values { get { return _values; } }

        public class BindingOptionsValues
        {
            public int Port { get; set; }
            public string Ip { get; set; }
            public string HostName { get; set; }
        }

        public class SslBindingOptionsValues
        {
            public BindingOptionsValues BindingOptions { get; set; }

            public X509FindType FindType { get; set; }

            public string FindName { get; set; }

            public CertLocation CertLocation { get; set; }

            public string FilePath { get; set; }

            public string PrivateKeyPassword { get; set; }

            public Action<IOfferCertificateOptions> CertOptions { get; set; }
        }
    }
}
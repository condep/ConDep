using System;

namespace ConDep.Dsl.Operations.Infrastructure.IIS.WebSite
{
    public class CertificateBindingOptions : IOfferCertificateBindingOptions
    {
        private CertificateBindingOptionsValues _values = new CertificateBindingOptionsValues();

        public IOfferCertificateBindingOptions FromCertStore(string commonName)
        {
            _values.CommonName = commonName;
            return this;
        }

        public IOfferCertificateBindingOptions FromFile(string filePath)
        {
            _values.FilePath = filePath;
            return this;
        }

        public CertificateBindingOptionsValues Values { get; set; }

        public class CertificateBindingOptionsValues
        {
            public string CommonName { get; set; }

            public string FilePath { get; set; }
        }
    }
}
using System.Collections.Generic;

namespace ConDep.Dsl.Operations.Infrastructure.IIS.WebSite
{
    public class CertificateOptions : IOfferCertificateOptions
    {
        private CertificateOptionsValues _values = new CertificateOptionsValues();

        public IOfferCertificateOptions AddPrivateKeyPermission(string user)
        {
            _values.PrivateKeyPermissions.Add(user);
            return this;
        }

        public CertificateOptionsValues Values { get { return _values; } }

        public class CertificateOptionsValues
        {
            private readonly List<string> _keyPermissions = new List<string>();

            public IList<string> PrivateKeyPermissions { get { return _keyPermissions; } }
        }

    }
}
using System;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using ConDep.Dsl.Operations.WebDeploy.Model;
using System.Linq;

namespace ConDep.Dsl
{
    public class CustomCertificateProvider : CompositeProvider
    {
        private readonly string _searchString;
        private readonly string _certFriendlyName;
        private readonly X509FindType _findType;
        private readonly string _certFile;
        private readonly bool _copyCertFromFile;

        public CustomCertificateProvider(string searchString, X509FindType findType)
        {
            _searchString = searchString;
            _findType = findType;
            _copyCertFromFile = false;
        }

        public CustomCertificateProvider(string searchString, string certFriendlyName)
        {
            _searchString = searchString;
            _certFriendlyName = certFriendlyName;
            _copyCertFromFile = false;
        }

        public CustomCertificateProvider(string certFile)
        {
            _certFile = certFile;
            _copyCertFromFile = true;
        }

        public override bool IsValid(Notification notification)
        {
            return File.Exists(_certFile);
        }

        public override void Configure()
        {
            if(_copyCertFromFile)
            {
                var cert = new X509Certificate2(_certFile);
                ConfigureCertInstall(cert);
            }
            else
            {
                var store = new X509Store(StoreName.My, StoreLocation.LocalMachine);
                store.Open(OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly);

                try
                {
                    var certs = new X509Certificate2Collection();
                
                    if(_certFriendlyName != null)
                    {
                        certs.AddRange(store.Certificates.Cast<X509Certificate2>().Where(cert => cert.FriendlyName == _certFriendlyName).ToArray());
                    }
                    else
                    {
                        certs.AddRange(store.Certificates.Find(_findType, _searchString, true));
                    }

                    if(certs.Count != 1)
                    {
                        if(certs.Count < 1)
                            throw new ApplicationException("Certificate not found");
                    
                        throw new ApplicationException("More than one certificate found in search");
                    }

                    ConfigureCertInstall(certs[0]);
                }
                finally
                {
                    store.Close();
                }
            }
        }

        private void ConfigureCertInstall(X509Certificate2 cert)
        {
            var certScript = string.Format("[byte[]]$byteArray = {0}; $myCert = new-object System.Security.Cryptography.X509Certificates.X509Certificate2(,$byteArray); ", string.Join(",", cert.GetRawCertData()));
            certScript += string.Format("$store = new-object System.Security.Cryptography.X509Certificates.X509Store('{0}', '{1}'); $store.open(“MaxAllowed”); $store.add($myCert); $store.close();", StoreName.My, StoreLocation.LocalMachine);
            Configure(p => p.PowerShell(certScript, o => o.WaitIntervalInSeconds(15).RetryAttempts(3)));
        }
    }
}
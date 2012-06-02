using System;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using ConDep.Dsl.Operations.WebDeploy.Model;

namespace ConDep.Dsl
{
    public class CustomCertificateProvider : CompositeProvider
    {
        private readonly string _searchString;
        private readonly X509ContentType _contentType;
        private readonly StoreName _storeName;
        private readonly StoreLocation _storeLocation;
        private readonly X509FindType _findType;
        private readonly string _certFile;
        private readonly bool _copyCertFromFile;

        public CustomCertificateProvider(string searchString, X509FindType findType)
        {
            _searchString = searchString;
            _findType = findType;
            _copyCertFromFile = false;
        }

        public CustomCertificateProvider(string certFile, X509ContentType contentType, StoreName storeName, StoreLocation storeLocation)
        {
            _certFile = certFile;
            _contentType = contentType;
            _storeName = storeName;
            _storeLocation = storeLocation;
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
                var certScript = string.Format("[byte[]]$byteArray = {0}; $myCert = new-object System.Security.Cryptography.X509Certificates.X509Certificate2(,$byteArray); ", string.Join(",", cert.GetRawCertData()));
                certScript += string.Format("$store = new-object System.Security.Cryptography.X509Certificates.X509Store('{0}', '{1}'); $store.open(“MaxAllowed”); $store.add($myCert); $store.close();", _storeName, _storeLocation);
                Configure(p => p.PowerShell(certScript, o => o.WaitIntervalInSeconds(15).RetryAttempts(3)));
            }
            else
            {
                throw new NotImplementedException();
            }
        }
    }
}
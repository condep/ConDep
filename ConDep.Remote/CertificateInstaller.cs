using System;
using System.Collections.Generic;
using System.IO;
using System.Security.AccessControl;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace ConDep.Remote
{
    public static class CertificateInstaller
    {
        public static void InstallCert(string filePath)
        {
            filePath = Environment.ExpandEnvironmentVariables(filePath);
            Console.WriteLine(string.Format("Installing certificate using file: [{0}].", filePath));
            var certificate = new X509Certificate2(filePath);
            AddCertToStore(certificate);
            RemoveCertFileFromDisk(filePath);
        }

        public static X509Certificate2 GetCertFromBase64(string base64Cert)
        {
            var cert = Convert.FromBase64String(base64Cert);
            return new X509Certificate2(cert);
        }

        public static void InstallCertFromBase64(string base64Cert)
        {
            var certificate = GetCertFromBase64(base64Cert);
            AddCertToStore(certificate);
        }

        public static void InstallCertToTrustedRoot(string filePath)
        {
            filePath = Environment.ExpandEnvironmentVariables(filePath);
            Console.Write("Installing certificate using file: [{0}].", filePath);
            var certificate = new X509Certificate2(filePath);
            var store = new X509Store(StoreName.Root, StoreLocation.LocalMachine);
            AddCertToStore(certificate, store);
            RemoveCertFileFromDisk(filePath);
        }


        public static void InstallPfx(string filePath, string password, string[] privateKeyUsers)
        {
            filePath = Environment.ExpandEnvironmentVariables(filePath);
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException(string.Format("File [{0}] not found.", filePath));
            }
            Console.WriteLine("Installing certificate using file: [{0}].", filePath);

            var cert = new X509Certificate2();
            cert.Import(filePath, password, X509KeyStorageFlags.PersistKeySet | X509KeyStorageFlags.Exportable | X509KeyStorageFlags.MachineKeySet );

            AddCertToStore(cert);

            if (cert.HasPrivateKey)
            {
                GrantUserReadAccessToCertificate(privateKeyUsers, cert);
            }

            RemoveCertFileFromDisk(filePath);
        }

        private static void RemoveCertFromStoreIfExist(X509Certificate2 cert)
        {
            var store = new X509Store(StoreName.My, StoreLocation.LocalMachine);
            try
            {
                store.Open(OpenFlags.ReadWrite);
                var certs = store.Certificates;
                var result = certs.Find(X509FindType.FindByThumbprint, cert.Thumbprint, false);

                if (result.Count > 1)
                {
                    throw new Exception("More than one certificate was found.");
                }
                else if (result.Count == 1)
                {
                    Console.WriteLine("Certificate was allready in store. Deleting from store now.");
                    var storeCert = result[0];
                    store.Remove(storeCert);
                }
            }
            finally
            {
                store.Close();
            }
        }

        private static void GrantUserReadAccessToCertificate(IEnumerable<string> privateKeyUsers, X509Certificate2 certificate)
        {
            if (privateKeyUsers == null)
                return;

            var store = new X509Store(StoreName.My, StoreLocation.LocalMachine);

            try
            {
                store.Open(OpenFlags.ReadWrite);

                var certs = store.Certificates;
                var result = certs.Find(X509FindType.FindByThumbprint, certificate.Thumbprint, false);

                if (result.Count == 1)
                {
                    var storeCert = result[0];

                    var rsa = storeCert.PrivateKey as RSACryptoServiceProvider;
                    var cspParams = new CspParameters(rsa.CspKeyContainerInfo.ProviderType, rsa.CspKeyContainerInfo.ProviderName,
                                                      rsa.CspKeyContainerInfo.KeyContainerName)
                    {
                        Flags = CspProviderFlags.UseExistingKey | CspProviderFlags.UseMachineKeyStore,
                        CryptoKeySecurity = rsa.CspKeyContainerInfo.CryptoKeySecurity
                    };

                    foreach (var user in privateKeyUsers)
                    {
                        cspParams.CryptoKeySecurity.AddAccessRule(new CryptoKeyAccessRule(user, CryptoKeyRights.GenericRead,
                                                                                          AccessControlType.Allow));
                    }

                    using (var rsa2 = new RSACryptoServiceProvider(cspParams))
                    {
                        // Only created to persist the rule change in the CryptoKeySecurity
                    }
                    return;
                }

                //store.Add(certificate);
                Console.WriteLine("Certificate installed in store.");
            }
            finally
            {
                store.Close();
            }
        }

        private static void AddCertToStore(X509Certificate2 certificate)
        {
            var store = new X509Store(StoreName.My, StoreLocation.LocalMachine);
            AddCertToStore(certificate, store);
        }

        private static void AddCertToStore(X509Certificate2 certificate, X509Store store)
        {
            try
            {
                store.Open(OpenFlags.ReadWrite);

                var certs = store.Certificates;
                var result = certs.Find(X509FindType.FindByThumbprint, certificate.Thumbprint, false);

                //Will only add cert to store if it doesn't already exist.
                if (result.Count > 0)
                {
                    store.Close();
                    return;
                }

                store.Add(certificate);
                Console.WriteLine("Certificate installed in store.");
            }
            finally
            {
                store.Close();
            }
        }

        private static void RemoveCertFileFromDisk(string filePath)
        {
            File.Delete(filePath);
            Console.WriteLine("Certificate removed from disk.");
        }
    }
}

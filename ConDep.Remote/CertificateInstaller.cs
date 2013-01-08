using System;
using System.IO;
using System.Security.Cryptography.X509Certificates;

namespace ConDep.Remote
{
    public static class CertificateInstaller
    {
        public static void InstallCert(string filePath)
        {
            Console.Write(string.Format("Installing certificate using file: [{0}].", filePath));
            var certificate = new X509Certificate2(filePath);
            AddCertToStore(certificate);
            RemoveCertFileFromDisk(filePath);
        }

        public static void InstallCertFromBase64(string base64Cert)
        {
            var cert = Convert.FromBase64String(base64Cert);
            var certificate = new X509Certificate2(cert);
            AddCertToStore(certificate);
        }

        public static void InstallCertToTrustedRoot(string filePath)
        {
            Console.Write(string.Format("Installing certificate using file: [{0}].", filePath));
            var certificate = new X509Certificate2(filePath);
            var store = new X509Store(StoreName.CertificateAuthority, StoreLocation.LocalMachine);
            AddCertToStore(certificate, store);
            RemoveCertFileFromDisk(filePath);
        }


        public static void InstallPfx(string filePath, string password)
        {
            Console.Write(string.Format("Installing certificate using file: [{0}].", filePath));

            var certCollection = new X509Certificate2Collection();
            certCollection.Import(filePath, password, X509KeyStorageFlags.PersistKeySet | X509KeyStorageFlags.Exportable );

            //var certificate = new X509Certificate2(filePath, password, storageFlags);

            foreach (var certificate in certCollection)
            {
                AddCertToStore(certificate);
            }
            RemoveCertFileFromDisk(filePath);
        }

        private static void AddCertToStore(X509Certificate2 certificate)
        {
            var store = new X509Store(StoreName.My, StoreLocation.LocalMachine);
            AddCertToStore(certificate, store);
        }

        private static void AddCertToStore(X509Certificate2 certificate, X509Store store)
        {

            store.Open(OpenFlags.ReadWrite);
            store.Add(certificate);
            store.Close();
            Console.Write("Certificate installed in store.");
        }

        private static void RemoveCertFileFromDisk(string filePath)
        {
            File.Delete(filePath);
            Console.Write("Certificate removed from disk.");
        }
    }
}

using System;
using System.IO;
using System.Security.Cryptography.X509Certificates;

namespace ConDep.Remote
{
    public static class CertificateInstaller
    {
        public static void InstallCert(string filePath)
        {
            Console.Write(string.Format("Installing certificate from [{0}].", filePath));
            var certificate = new X509Certificate2(filePath);
            AddCertToStore(certificate);
            RemoveCertFileFromDisk(filePath);
        }

        public static void InstallCertToTrustedRoot(string filePath)
        {
            Console.Write(string.Format("Installing certificate from [{0}].", filePath));
            var certificate = new X509Certificate2(filePath);
            var store = new X509Store(StoreName.CertificateAuthority, StoreLocation.LocalMachine);
            store.Open(OpenFlags.ReadWrite);
            store.Add(certificate);
            store.Close();
            Console.Write("Certificate installed in store.");
            RemoveCertFileFromDisk(filePath);
        }


        public static void InstallPfx(string filePath, string password, X509KeyStorageFlags storageFlags)
        {
            Console.Write(string.Format("Installing certificate from [{0}].", filePath));
            var certificate = new X509Certificate2(filePath, password, storageFlags);
            AddCertToStore(certificate);
            RemoveCertFileFromDisk(filePath);
        }

        private static void AddCertToStore(X509Certificate2 certificate)
        {
            var store = new X509Store(StoreName.My, StoreLocation.LocalMachine);
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

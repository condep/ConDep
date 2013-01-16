using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using ConDep.Dsl.Builders;
using ConDep.Dsl.Operations.Infrastructure.Certificate;
using ConDep.Dsl.Operations.Infrastructure.IIS.WebSite;
using ConDep.Dsl.SemanticModel;

namespace ConDep.Dsl.Operations.Application.Deployment.Certificate
{
    public class CertificateFromStoreOperation : RemoteCompositeOperation
    {
        private readonly X509FindType _findType;
        private readonly string _findValue;
        private readonly CertificateOptions _certOptions;

        public CertificateFromStoreOperation(X509FindType findType, string findValue, CertificateOptions certOptions = null)
        {
            _findType = findType;
            _findValue = findValue;
            _certOptions = certOptions;
        }

        public override void Configure(IOfferRemoteComposition server)
        {

            var store = new X509Store(StoreName.My, StoreLocation.LocalMachine);
            try
            {
                store.Open(OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly);
                var certs = store.Certificates;

                var findResult = certs.Find(_findType, _findValue, false);
                if (findResult == null || findResult.Count == 0)
                {
                    throw new ConDepCertificateNotFoundException(string.Format("Certificate with find type [{0}] and term [{1}] not found.", _findType, _findValue));
                }

                if (findResult.Count > 1)
                {
                    throw new ConDepCertificateNotFoundException(string.Format("Certificate with find type [{0}] and term [{1}] returned {2} certificates. Please narrow your search to only return one certificate.", _findType, _findValue, findResult.Count));
                }

                var cert = findResult[0];

                if (cert.HasPrivateKey)
                {
                    var guid = Guid.NewGuid();
                    var destPath = string.Format(@"%temp%\{0}.pfx", guid);
                    var sourcePath = Path.Combine(Path.GetTempPath(), guid + ".pfx");

                    const string password = "%se65#1s)=3";
                    var exportedCert = cert.Export(X509ContentType.Pkcs12, password);
                    File.WriteAllBytes(sourcePath, exportedCert);

                    server.Deploy.File(sourcePath, destPath);

                    var formattedUserArray = _certOptions.Values.PrivateKeyPermissions.Select(user => "'" + user + "'").ToList();
                    var users = string.Join(",", formattedUserArray);
                    var psUserArray = string.Format("@({0})", users);

                    server.ExecuteRemote.PowerShell("$path='" + destPath + "'; $password='" + password + "'; $privateKeyUsers = " + psUserArray + "; [ConDep.Remote.CertificateInstaller]::InstallPfx($path, $password, $privateKeyUsers);", opt => opt.RequireRemoteLib().WaitIntervalInSeconds(10));
                }
                else
                {
                    var base64Cert = Convert.ToBase64String(cert.RawData);
                    server.ExecuteRemote.PowerShell(string.Format("[ConDep.Remote.CertificateInstaller]::InstallCertFromBase64('{0}');", base64Cert), opt => opt.RequireRemoteLib().WaitIntervalInSeconds(20));
                }
            }
            finally
            {
                store.Close();
            }
        }

        public override string Name
        {
            get { return "CertificateFromStore"; }
        }

        public override bool IsValid(Notification notification)
        {
            return true;
        }
    }
}
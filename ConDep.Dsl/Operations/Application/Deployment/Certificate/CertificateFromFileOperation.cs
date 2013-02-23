using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using ConDep.Dsl.Operations.Infrastructure.IIS.WebSite;
using ConDep.Dsl.SemanticModel;

namespace ConDep.Dsl.Operations.Application.Deployment.Certificate
{
    public class CertificateFromFileOperation : RemoteCompositeOperation
    {
        private readonly string _path;
        private readonly string _password;
        private readonly CertificateOptions _certOptions;

        public CertificateFromFileOperation(string path, string password, CertificateOptions certOptions = null)
        {
            _path = path;
            _password = password;
            _certOptions = certOptions;
        }

        public override void Configure(IOfferRemoteComposition server)
        {
            var path = Path.GetFullPath(_path);
            var cert = string.IsNullOrWhiteSpace(_password) ? new X509Certificate2(path) : new X509Certificate2(path, _password);

            if(cert.HasPrivateKey)
            {
                string psUserArray = "@()";
                if(_certOptions != null && _certOptions.Values.PrivateKeyPermissions.Count > 0)
                {
                    var formattedUserArray = _certOptions.Values.PrivateKeyPermissions.Select(user => "'" + user + "'").ToList();
                    var users = string.Join(",", formattedUserArray);
                    psUserArray = string.Format("@({0})", users);
                }

                var destPath = string.Format(@"%temp%\{0}.pfx", Guid.NewGuid());
                server.Deploy.File(path, destPath);
                server.ExecuteRemote.PowerShell("$path='" + destPath + "'; $password='" + _password + "'; $privateKeyUsers = " + psUserArray + "; [ConDep.Remote.CertificateInstaller]::InstallPfx($path, $password, $privateKeyUsers);", opt => opt.RequireRemoteLib().WaitIntervalInSeconds(30));
            }
            else
            {
                var base64Cert = Convert.ToBase64String(cert.RawData);
                server.ExecuteRemote.PowerShell(string.Format("[ConDep.Remote.CertificateInstaller]::InstallCertFromBase64('{0}');", base64Cert), opt => opt.RequireRemoteLib().WaitIntervalInSeconds(20));
            }
        }

        public override string Name
        {
            get { return "CertificateFromFile"; }
        }

        public override bool IsValid(Notification notification)
        {
            return true;
        }
    }
}
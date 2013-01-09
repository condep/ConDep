using System;
using System.Security.Cryptography.X509Certificates;
using ConDep.Dsl.Builders;
using ConDep.Dsl.SemanticModel;

namespace ConDep.Dsl.Operations.Application.Deployment.Certificate
{
    public class CertificateFromFileOperation : RemoteCompositeOperation
    {
        private readonly string _path;
        private readonly string _password;

        public CertificateFromFileOperation(string path, string password)
        {
            _path = path;
            _password = password;
        }

        public override void Configure(IOfferRemoteComposition server)
        {
            var cert = string.IsNullOrWhiteSpace(_password) ? new X509Certificate2(_path) : new X509Certificate2(_path, _password);

            if(cert.HasPrivateKey)
            {
                var destPath = string.Format(@"%temp%\{0}.pfx", Guid.NewGuid());
                server.Deploy.File(_path, destPath);
                server.ExecuteRemote.PowerShell("$path='" + destPath + "'; $password='" + _password + "'; [ConDep.Remote.CertificateInstaller]::InstallPfx($path, $password);", opt => opt.RequireRemoteLib().WaitIntervalInSeconds(10));
            }
            else
            {
                var base64Cert = Convert.ToBase64String(cert.RawData);
                server.ExecuteRemote.PowerShell(string.Format("[ConDep.Remote.CertificateInstaller]::InstallCertFromBase64('{0}');", base64Cert), opt => opt.RequireRemoteLib().WaitIntervalInSeconds(20));
            }
        }

        public override string Name
        {
            get { return "Certificate"; }
        }

        public override bool IsValid(Notification notification)
        {
            return true;
        }
    }
}
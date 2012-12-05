using ConDep.Dsl.Builders;
using ConDep.Dsl.SemanticModel;

namespace ConDep.Dsl.Operations.Application.Deployment.Certificate
{
    public class CertificateOperation : RemoteCompositeOperation
    {
        private readonly string _path;
        private readonly string _password;

        public CertificateOperation(string path, string password)
        {
            _path = path;
            _password = password;
        }

        public override void Configure(IOfferRemoteOperations server)
        {
            var destPath = @"%temp%\condepcert.pfx";
            server.Deploy.File(_path, destPath);
            server.ExecuteRemote.PowerShell("$path='" + destPath + "'; $password='" + _password + "'; $storageFlags=[System.Security.Cryptography.X509Certificates.X509KeyStorageFlags]::Exportable; [ConDep.Remote.CertificateInstaller]::InstallPfx($path, $password, $storageFlags);", opt => opt.RequireRemoteLib().WaitIntervalInSeconds(10));
        }

        public override bool IsValid(Notification notification)
        {
            return true;
        }
    }
}
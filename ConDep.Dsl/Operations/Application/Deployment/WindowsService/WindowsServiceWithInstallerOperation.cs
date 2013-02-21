using System.IO;
using ConDep.Dsl.SemanticModel;

namespace ConDep.Dsl.Operations.Application.Deployment.WindowsService
{
    public class WindowsServiceWithInstallerOperation : WindowsServiceOperationBase
    {
        private readonly string _installerParams;

        public WindowsServiceWithInstallerOperation(string serviceName, string displayName, string sourceDir, string destDir, string relativeExePath, string installerParams, WindowsServiceOptions.WindowsServiceOptionValues options)
            : base(serviceName, displayName, sourceDir, destDir, relativeExePath, options)
        {
            _installerParams = installerParams;
        }

        public override string Name
        {
            get { return "Windows Service With Installer"; }
        }

        public override bool IsValid(Notification notification)
        {
            return true;
        }

        protected override void ConfigureInstallService(IOfferRemoteComposition server)
        {
            var installCmd = string.Format("{0} {1}", Path.Combine(_destDir, _relativeExePath), _installerParams);
            server.ExecuteRemote.DosCommand(installCmd, opt => opt.WaitIntervalInSeconds(60));
        }
    }
}
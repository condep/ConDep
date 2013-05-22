using System.IO;
using ConDep.Dsl.SemanticModel;

namespace ConDep.Dsl.Operations.Application.Deployment.WindowsService
{
    public class WindowsServiceOperation : WindowsServiceOperationBase
    {
        public WindowsServiceOperation(string serviceName, string displayName, string sourceDir, string destDir, string relativeExePath, WindowsServiceOptions.WindowsServiceOptionValues values)
            : base(serviceName, displayName, sourceDir, destDir, relativeExePath, values)
        {
        }

        public override string Name
        {
            get { return "Windows Service"; }
        }

        public override bool IsValid(Notification notification)
        {
            return true;
        }

        protected override void ConfigureInstallService(IOfferRemoteComposition server)
        {
            var installCmd = string.Format("New-ConDepWinService '{0}' '{1}' {2} {3} {4}",
                                           _serviceName,
                                           Path.Combine(_destDir, _relativeExePath) + " " + _values.ExeParams,
                                           string.IsNullOrWhiteSpace(_displayName) ? "$null" : ("'" + _displayName + "'"),
                                           string.IsNullOrWhiteSpace(_values.Description)
                                               ? "$null"
                                               : ("'" + _values.Description + "'"),
                                           _values.StartupType.HasValue ? "'" + _values.StartupType + "'" : "$null"
                );

            server.ExecuteRemote.PowerShell(installCmd);
        }
    }
}
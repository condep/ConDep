using System.IO;
using ConDep.Dsl.SemanticModel;

namespace ConDep.Dsl.Operations.Application.Deployment.WindowsService
{
    public class WindowsServiceOperation : WindowsServiceOperationBase
    {
        public WindowsServiceOperation(string serviceName, string sourceDir, string destDir, string relativeExePath, string displayName, WindowsServiceOptions.WindowsServiceOptionValues values) : base(serviceName, sourceDir, destDir, relativeExePath, displayName, values)
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
            var installCmd = string.Format("New-ConDepWinService '{0}' '{1}' {2} {3} {4} {5} {6}",
                                           _serviceName,
                                           Path.Combine(_destDir, _relativeExePath) + " " + _values.ExeParams,
                                           string.IsNullOrWhiteSpace(_displayName) ? "$null" : ("'" + _displayName + "'"),
                                           string.IsNullOrWhiteSpace(_values.Description)
                                               ? "$null"
                                               : ("'" + _values.Description + "'"),
                                           string.IsNullOrWhiteSpace(_values.UserName)
                                               ? "$null"
                                               : ("'" + _values.UserName + "'"),
                                           string.IsNullOrWhiteSpace(_values.Password)
                                               ? "$null"
                                               : ("'" + _values.UserName + "'"),
                                           _values.StartupType.HasValue ? "'" + _values.StartupType + "'" : "$null"
                );

            server.ExecuteRemote.PowerShell(installCmd, opt => opt.WaitIntervalInSeconds(60));
        }
    }
}
using System.IO;
using ConDep.Dsl.WebDeployProviders.Deployment.NServiceBus;

namespace ConDep.Dsl.Experimental.Application.Dsl.Remote
{
    public class NServiceBusOperation : RemoteCompositeOperation
    {
        internal const string SERVICE_CONTROLLER_EXE = @"C:\WINDOWS\system32\sc.exe";
        private string _serviceInstallerName = "NServiceBus.Host.exe";

        public NServiceBusOperation(string path, string destDir, string serviceName)
        {
            SourcePath = Path.GetFullPath(path);
            ServiceName = serviceName;
            DestinationPath = destDir;
        }

        public string ServicePassword { get; set; }
        public string ServiceUserName { get; set; }

        internal string ServiceName { get; set; }
        internal string ServiceGroup { get; set; }
        internal string Profile { get; set; }
        internal int? ServiceFailureResetInterval { get; set; }
        internal int? ServiceRestartDelay { get; set; }
        internal bool IgnoreFailureOnServiceStartStop { get; set; }

        public string ServiceInstallerName
        {
            get { return _serviceInstallerName; }
            set { _serviceInstallerName = value; }
        }

        public override void Configure(IOfferRemoteOptions server)
        {
            CopyPowerShellScriptsToTarget(server.Deploy);

            var install = string.Format("{0} /install /serviceName:\"{1}\" /displayName:\"{1}\" {2}", Path.Combine(DestinationPath, ServiceInstallerName), ServiceName, Profile);

            var serviceFailureCommand = "";
            var serviceConfigCommand = "";

            if (HasServiceFailureOptions)
            {
                var serviceResetOption = ServiceFailureResetInterval.HasValue ? "reset= " + ServiceFailureResetInterval.Value : "";
                var serviceRestartDelayOption = ServiceRestartDelay.HasValue ? "actions= restart/" + ServiceRestartDelay.Value : "";

                serviceFailureCommand = string.Format("{0} failure \"{1}\" {2} {3}", SERVICE_CONTROLLER_EXE, ServiceName, serviceResetOption, serviceRestartDelayOption);
            }

            if (HasServiceConfigOptions)
            {
                var userNameOption = !string.IsNullOrWhiteSpace(ServiceUserName) ? "obj= \"" + ServiceUserName + "\"" : "";
                var passwordOption = !string.IsNullOrWhiteSpace(ServicePassword) ? "password= \"" + ServicePassword + "\"" : "";
                var groupOption = !string.IsNullOrWhiteSpace(ServiceGroup) ? "group= \"" + ServiceGroup + "\"" : "";

                serviceConfigCommand = string.Format("{0} config \"{1}\" {2} {3} {4}", SERVICE_CONTROLLER_EXE, ServiceName, userNameOption, passwordOption, groupOption);
            }

            var remove = string.Format(". $env:temp\\NServiceBus.ps1; remove-nsbservice {0}", ServiceName);
            server.ExecuteRemote.PowerShell(remove, o => o.ContinueOnError(IgnoreFailureOnServiceStartStop).WaitIntervalInSeconds(10).RetryAttempts(10));
            server.Deploy.Directory(SourcePath, DestinationPath);

            //Allow continue on error??
            server.ExecuteRemote.DosCommand(install, false, opt => opt.WaitIntervalInSeconds(10));
            if (!string.IsNullOrWhiteSpace(serviceFailureCommand)) server.ExecuteRemote.DosCommand(serviceFailureCommand);
            if (!string.IsNullOrWhiteSpace(serviceConfigCommand)) server.ExecuteRemote.DosCommand(serviceConfigCommand);

            var start = string.Format(". $env:temp\\NServiceBus.ps1; start-nsbservice {0}", ServiceName);
            server.ExecuteRemote.PowerShell(start, o => o.WaitIntervalInSeconds(10).RetryAttempts(10).ContinueOnError(IgnoreFailureOnServiceStartStop));
        }

        public override bool IsValid(Notification notification)
        {
            throw new System.NotImplementedException();
        }

        private void CopyPowerShellScriptsToTarget(IOfferRemoteDeployment deploy)
        {
            var filePath = ConDepResourceFiles.GetFilePath(GetType().Namespace, "NServiceBus.ps1");
            deploy.File(filePath, @"%temp%\NServiceBus.ps1");
        }

        private bool HasServiceConfigOptions
        {
            get { return !string.IsNullOrWhiteSpace(ServiceUserName) || !string.IsNullOrWhiteSpace(ServicePassword) || !string.IsNullOrWhiteSpace(ServiceGroup); }
        }

        private bool HasServiceFailureOptions
        {
            get { return ServiceFailureResetInterval.HasValue || ServiceRestartDelay.HasValue; }
        }

    }
}
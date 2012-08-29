using ConDep.Dsl.Core;

namespace ConDep.Dsl
{
    public class AppPoolInfrastructureProvider : WebDeployCompositeProviderBase
    {
        private readonly string _appPoolName;
        private readonly ApplicationPool _appPool = new ApplicationPool();

        public AppPoolInfrastructureProvider(string appPoolName)
        {
            _appPoolName = appPoolName;
        }

        public AppPoolInfrastructureProvider(string appPoolName, ApplicationPool appPool)
        {
            _appPoolName = appPoolName;
            _appPool = appPool;
        }

        public ApplicationPool AppPool { get { return _appPool; } }

        public override bool IsValid(Notification notification)
        {
            return !string.IsNullOrWhiteSpace(_appPoolName);
        }

        public override void Configure(DeploymentServer server)
        {
            var psCommand = string.Format("Set-Location IIS:\\AppPools; try {{ Remove-WebAppPool '{0}' }} catch {{ }}; $newAppPool = New-WebAppPool '{0}'; ", _appPoolName);

            if(_appPool != null)
            {
                psCommand += _appPool.Enable32Bit != null ? string.Format("$newAppPool.enable32BitAppOnWin64 = {0}; ", _appPool.Enable32Bit.Value ? "$true" : "$false") : "";
                psCommand += _appPool.IdentityUsername != null ? string.Format("$newAppPool.processModel.identityType = 'SpecificUser'; $newAppPool.processModel.username = '{0}'; $newAppPool.processModel.password = '{1}'; ", _appPool.IdentityUsername, _appPool.IdentityPassword) : "";
                psCommand += _appPool.IdleTimeoutInMinutes != null ? string.Format("$newAppPool.processModel.idleTimeout = [TimeSpan]::FromMinutes({0}); ", _appPool.IdleTimeoutInMinutes) : "";
                psCommand += _appPool.LoadUserProfile != null ? string.Format("$newAppPool.processModel.loadUserProfile = {0}; ", _appPool.LoadUserProfile.Value ? "$true" : "$false") : "";
                psCommand += _appPool.ManagedPipeline != null ? string.Format("$newAppPool.managedPipelineMode = '{0}'; ", _appPool.ManagedPipeline) : "";
                psCommand += _appPool.NetFrameworkVersion != null ? string.Format("$newAppPool.managedRuntimeVersion = '{0}'; ", ExtractNetFrameworkVersion()) : "";
                psCommand += _appPool.RecycleTimeInMinutes != null ? string.Format("$newAppPool.recycling.periodicrestart.time = [TimeSpan]::FromMinutes({0}); ", _appPool.RecycleTimeInMinutes) : "";
            }

            psCommand += "$newAppPool | set-item;";
            Configure<ProvideForInfrastructure>(server, AddChildProvider, po => po.PowerShell("Import-Module WebAdministration; " + psCommand, o => o.WaitIntervalInSeconds(2).RetryAttempts(20)));
        }

        private string ExtractNetFrameworkVersion()
        {
            switch (_appPool.NetFrameworkVersion)
            {
                case NetFrameworkVersion.Net1_0:
                    return "v1.0";
                case NetFrameworkVersion.Net1_1:
                    return "v1.1";
                case NetFrameworkVersion.Net2_0:
                    return "v2.0";
                case NetFrameworkVersion.Net4_0:
                    return "v4.0";
                case NetFrameworkVersion.Net5_0:
                    return "v5.0";
                default:
                    throw new UnknowNetFrameworkException("Framework version unknown to ConDep.");
            }

        }

    }
}
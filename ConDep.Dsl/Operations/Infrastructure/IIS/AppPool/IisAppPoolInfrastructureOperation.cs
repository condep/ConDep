using ConDep.Dsl.Builders;
using ConDep.Dsl.SemanticModel;

namespace ConDep.Dsl.Operations.Infrastructure.IIS.AppPool
{
    public class IisAppPoolInfrastructureOperation : RemoteCompositeOperation
    {
        private readonly string _appPoolName;
        private readonly IisAppPoolOptions _appPoolOptions = new IisAppPoolOptions();

        public IisAppPoolInfrastructureOperation(string appPoolName)
        {
            _appPoolName = appPoolName;
        }

        public IisAppPoolInfrastructureOperation(string appPoolName, IisAppPoolOptions appPoolOptions) 
        {
            _appPoolName = appPoolName;
            _appPoolOptions = appPoolOptions;
        }

        public IisAppPoolOptions AppPoolOptions { get { return _appPoolOptions; } }

        public override string Name
        {
            get { return "IIS Application Pool"; }
        }

        public override bool IsValid(Notification notification)
        {
            return !string.IsNullOrWhiteSpace(_appPoolName);
        }

        public override void Configure(IOfferRemoteComposition server)
        {
            var psCommand = string.Format("Set-Location IIS:\\AppPools; try {{ Remove-WebAppPool '{0}' }} catch {{ }}; $newAppPool = New-WebAppPool '{0}'; ", _appPoolName);

            if (_appPoolOptions != null)
            {
                psCommand += _appPoolOptions.Enable32Bit != null ? string.Format("$newAppPool.enable32BitAppOnWin64 = {0}; ", _appPoolOptions.Enable32Bit.Value ? "$true" : "$false") : "";
                psCommand += _appPoolOptions.IdentityUsername != null ? string.Format("$newAppPool.processModel.identityType = 'SpecificUser'; $newAppPool.processModel.username = '{0}'; $newAppPool.processModel.password = '{1}'; ", _appPoolOptions.IdentityUsername, _appPoolOptions.IdentityPassword) : "";
                psCommand += _appPoolOptions.IdleTimeoutInMinutes != null ? string.Format("$newAppPool.processModel.idleTimeout = [TimeSpan]::FromMinutes({0}); ", _appPoolOptions.IdleTimeoutInMinutes) : "";
                psCommand += _appPoolOptions.LoadUserProfile != null ? string.Format("$newAppPool.processModel.loadUserProfile = {0}; ", _appPoolOptions.LoadUserProfile.Value ? "$true" : "$false") : "";
                psCommand += _appPoolOptions.ManagedPipeline != null ? string.Format("$newAppPool.managedPipelineMode = '{0}'; ", _appPoolOptions.ManagedPipeline) : "";
                psCommand += _appPoolOptions.NetFrameworkVersion != null ? string.Format("$newAppPool.managedRuntimeVersion = '{0}'; ", ExtractNetFrameworkVersion()) : "";
                psCommand += _appPoolOptions.RecycleTimeInMinutes != null ? string.Format("$newAppPool.recycling.periodicrestart.time = [TimeSpan]::FromMinutes({0}); ", _appPoolOptions.RecycleTimeInMinutes) : "";
            }

            psCommand += "$newAppPool | set-item;";
            server.ExecuteRemote.PowerShell("Import-Module WebAdministration; " + psCommand, o => o.WaitIntervalInSeconds(2).RetryAttempts(20));
        }

        private string ExtractNetFrameworkVersion()
        {
            switch (_appPoolOptions.NetFrameworkVersion)
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
                    throw new ConDepUnknowNetFrameworkException("Framework version unknown to ConDep.");
            }

        }

    }
}
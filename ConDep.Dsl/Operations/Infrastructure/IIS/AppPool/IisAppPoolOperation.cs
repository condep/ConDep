using System.Collections.Generic;
using System.Globalization;
using ConDep.Dsl.Builders;
using ConDep.Dsl.Operations.Application.Execution.PowerShell;
using ConDep.Dsl.Resources;
using ConDep.Dsl.SemanticModel;

namespace ConDep.Dsl.Operations.Infrastructure.IIS.AppPool
{
    public class IisAppPoolOperation : RemoteCompositeInfrastructureOperation
    {
        private readonly string _appPoolName;
        private readonly IisAppPoolOptions.IisAppPoolOptionsValues _appPoolOptions;
        private List<string> _scriptPaths = new List<string>();

        public IisAppPoolOperation(string appPoolName)
        {
            _appPoolName = appPoolName;
        }

        public IisAppPoolOperation(string appPoolName, IisAppPoolOptions.IisAppPoolOptionsValues appPoolOptions) 
        {
            _appPoolName = appPoolName;
            _appPoolOptions = appPoolOptions;
        }

        public override string Name
        {
            get { return "IIS Application Pool"; }
        }

        public override bool IsValid(Notification notification)
        {
            return !string.IsNullOrWhiteSpace(_appPoolName);
        }

        public override void Configure(IOfferRemoteComposition server, IOfferInfrastructure require)
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
            string appPoolOptions;

            if(_appPoolOptions != null)
            {
                appPoolOptions = string.Format("$appPoolOptions = @{{Enable32Bit=${0}; IdentityUsername='{1}'; IdentityPassword='{2}'; IdleTimeoutInMinutes={3}; LoadUserProfile=${4}; ManagedPipeline={5}; NetFrameworkVersion={6}; RecycleTimeInMinutes={7}}};"
                    , _appPoolOptions.Enable32Bit.HasValue ? _appPoolOptions.Enable32Bit.Value.ToString() : "false"
                    , _appPoolOptions.IdentityUsername
                    , _appPoolOptions.IdentityPassword
                    , _appPoolOptions.IdleTimeoutInMinutes.HasValue ? _appPoolOptions.IdleTimeoutInMinutes.Value.ToString(CultureInfo.InvariantCulture.NumberFormat) : "$null"
                    , _appPoolOptions.LoadUserProfile.HasValue ? _appPoolOptions.LoadUserProfile.Value.ToString() : "false"
                    , _appPoolOptions.ManagedPipeline.HasValue ? "'" + _appPoolOptions.ManagedPipeline.Value + "'" : "$null"
                    , _appPoolOptions.NetFrameworkVersion.HasValue ? "'" + ExtractNetFrameworkVersion() + "'" : "$null"
                    , _appPoolOptions.RecycleTimeInMinutes.HasValue ? _appPoolOptions.RecycleTimeInMinutes.Value.ToString(CultureInfo.InvariantCulture.NumberFormat) : "$null"
                    );
            }
            else
            {
                appPoolOptions = "$appPoolOptions = $null;";
            }
            server.ExecuteRemote.PowerShell(string.Format(@"{0} New-ConDepAppPool '{1}' $appPoolOptions;", appPoolOptions, _appPoolName), psOptions => psOptions.WaitIntervalInSeconds(30));
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
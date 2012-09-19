using System.IO;
using ConDep.Dsl;
using ConDep.Dsl.LoadBalancer;
using ConDep.Dsl.WebDeploy;

namespace ConDep.LoadBalancer.Arr
{
    public class ApplicationRequestRoutingProvider : WebDeployCompositeProviderBase
    {
        private readonly LoadBalanceState _state;
        private readonly string _serverNameToChangeStateOn;

        public ApplicationRequestRoutingProvider(LoadBalanceState state, string serverNameToChangeStateOn)
        {
            _state = state;
            _serverNameToChangeStateOn = serverNameToChangeStateOn;
        }

        public override bool IsValid(Notification notification)
        {
            return true;
        }

        public override void Configure(DeploymentServer arrServer)
        {
            var condition = WebDeployExecuteCondition<ProvideForInfrastructure>.IsFailure(p => p.PowerShell("import-module ApplicationRequestRouting"));

            DeployPsCmdLet(arrServer, condition);
            Execute(_state, arrServer, _serverNameToChangeStateOn);
            RemovePsCmdLet(arrServer, condition);
        }

        private void DeployPsCmdLet(DeploymentServer server, WebDeployExecuteCondition<ProvideForInfrastructure> condition)
        {
            var dir = Path.Combine(Path.GetDirectoryName(GetType().Assembly.Location), "ArrLoadBalancer");
            Configure<ProvideForDeployment, ProvideForInfrastructure>(server, p => p.CopyDir(dir, @"%temp%\ApplicationRequestRouting"), condition);
        }

        private void Execute(LoadBalanceState state, DeploymentServer server, string serverNameToChangeStateOn)
        {
            Configure<ProvideForInfrastructure>(server, p => p.PowerShell(string.Format(@"import-module $env:temp\ApplicationRequestRouting; Set-WebFarmServerState -State {0} -Name {1} -UseDnsLookup;", state.ToString(), serverNameToChangeStateOn), o => o.WaitIntervalInSeconds(10)));
        }

        private void RemovePsCmdLet(DeploymentServer arrServer, WebDeployExecuteCondition<ProvideForInfrastructure> condition)
        {
            Configure<ProvideForInfrastructure, ProvideForInfrastructure>(arrServer, p => p.PowerShell(@"remove-item $env:temp\ApplicationRequestRouting -force -recurse"), condition);
        }

        private void Condition(ProvideForInfrastructure c)
        {
            c.PowerShell("import-module ApplicationRequestRouting");
        }
    }
}
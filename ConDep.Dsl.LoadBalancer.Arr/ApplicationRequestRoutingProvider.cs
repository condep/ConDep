using System.IO;
using ConDep.Dsl;
using ConDep.Dsl.LoadBalancer;
using ConDep.Dsl.Model.Config;
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

        public override void Configure(ServerConfig arrServer)
        {
            DeployPsCmdLet(arrServer);
            Execute(_state, arrServer, _serverNameToChangeStateOn);
        }

        private void DeployPsCmdLet(ServerConfig server)
        {
            var condition = WebDeployExecuteCondition<ProvideForInfrastructure>.IsFailure(p => p.PowerShell("import-module ApplicationRequestRouting"));

            var dir = Path.Combine(Path.GetDirectoryName(GetType().Assembly.Location), "ArrLoadBalancer");
            Configure<ProvideForDeployment, ProvideForInfrastructure>(server, p => p.CopyDir(dir, @"%temp%\ApplicationRequestRouting"), condition);
        }

        private void Execute(LoadBalanceState state, ServerConfig server, string serverNameToChangeStateOn)
        {
            Configure<ProvideForInfrastructure>(server, p => 
                p.PowerShell(string.Format(@"import-module $env:temp\ApplicationRequestRouting; Set-WebFarmServerState -State {0} -Name {1} -UseDnsLookup;", 
                             state.ToString(), 
                             serverNameToChangeStateOn), o =>{
                                                                o.WaitIntervalInSeconds(10);
                                                                o.RetryAttempts(20);
                                                             }));
        }
    }
}
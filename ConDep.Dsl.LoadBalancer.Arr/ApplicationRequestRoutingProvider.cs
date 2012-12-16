using System.IO;
using ConDep.Dsl.Builders;
using ConDep.Dsl.Operations;
using ConDep.Dsl.Operations.LoadBalancer;
using ConDep.Dsl.SemanticModel;

namespace ConDep.LoadBalancer.Arr
{
    public class ApplicationRequestRoutingProvider : RemoteCompositeOperation
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

        public override void Configure(IOfferRemoteComposition server)
        {
            DeployPsCmdLet(server);
            Execute(_state, server, _serverNameToChangeStateOn);
        }

        private void DeployPsCmdLet(IOfferRemoteOperations server)
        {
            //var condition = WebDeployExecuteCondition<ProvideForInfrastructure>.IsFailure(p => p.PowerShell("import-module ApplicationRequestRouting"));

            var dir = Path.Combine(Path.GetDirectoryName(GetType().Assembly.Location), "ArrLoadBalancer");
            server.Deploy.Directory(dir, @"%temp%\ApplicationRequestRouting");
        }

        private void Execute(LoadBalanceState state, IOfferRemoteOperations server, string serverNameToChangeStateOn)
        {
                server.ExecuteRemote.PowerShell(string.Format(@"import-module $env:temp\ApplicationRequestRouting; Set-WebFarmServerState -State {0} -Name {1} -UseDnsLookup;",
                             state.ToString(),
                             serverNameToChangeStateOn), o =>
                             {
                                 o.WaitIntervalInSeconds(10);
                                 o.RetryAttempts(20);
                             });
        }
    }
}
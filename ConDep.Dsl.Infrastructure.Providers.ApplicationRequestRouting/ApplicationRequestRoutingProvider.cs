using ConDep.Dsl.Core;

namespace ConDep.Dsl.Infrastructure.Providers.ApplicationRequestRouting
{
    public class ApplicationRequestRoutingProvider : WebDeployCompositeProvider 
    {
        private readonly LoadBalanceState _state;

        public ApplicationRequestRoutingProvider(LoadBalanceState state)
        {
            _state = state;
        }

        public override bool IsValid(Notification notification)
        {
            return true;
        }

        public override void Configure(DeploymentServer server)
        {
            //if(ArrPsCmdletExist())
            //{
            //    Execute();
            //}
            DeployPsCmdLet();
            Execute(_state, server.ServerName);
            RemovePsCmdLet();
        }

        private void DeployPsCmdLet()
        {
            Configure(p => p.CopyDir(@"C:\GitHub\ConDep\ConDep.PowerShell.ApplicationRequestRouting\bin\Release", opt => opt.DestinationDir(@"C:\ConDepTemp\ApplicationRequestRouting")));
        }

        private void Execute(LoadBalanceState state, string serverName)
        {
            Configure(p => p.PowerShell(string.Format(@"import-module C:\ConDepTemp\ApplicationRequestRouting; Set-WebFarmServerState -State {0} -Name {1} -UseDnsLookup;", state.ToString(), serverName)));
        }

        private void RemovePsCmdLet()
        {
            Configure(p => p.PowerShell(@"remove-item C:\ConDepTemp\ApplicationRequestRouting -force -recurse"));
        }

        //private bool ArrPsCmdletExist()
        //{
        //    Configure(p => p.PowerShell("$mod = get-module -ListAvailable -Name ApplicationRequestRouting; if($mod -eq $null) { throw }"));
        //    return exist;
        //}
    }
}
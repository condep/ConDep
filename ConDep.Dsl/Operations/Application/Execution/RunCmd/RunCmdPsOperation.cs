using ConDep.Dsl.SemanticModel;

namespace ConDep.Dsl.Operations.Application.Execution.RunCmd
{
    public class RunCmdPsOperation : RemoteCompositeOperation
    {
        private readonly string _cmd;
        private readonly RunCmdOptions.RunCmdOptionValues _values;

        public RunCmdPsOperation(string cmd, RunCmdOptions.RunCmdOptionValues values = null)
        {
            _cmd = cmd;
            _values = values;
        }

        public override string Name
        {
            get { return "Run Command (PS)"; }
        }

        public override bool IsValid(Notification notification)
        {
            return true;
        }

        public override void Configure(IOfferRemoteComposition server)
        {
            server.ExecuteRemote.PowerShell("cmd /c " + _cmd);
        }
    }
}
using ConDep.Dsl.SemanticModel;

namespace ConDep.Dsl.Operations.Application.Execution.RunCmd
{
    public class RunCmdPsOperation : RemoteCompositeOperation
    {
        private readonly string _cmd;

        public RunCmdPsOperation(string cmd)
        {
            _cmd = cmd;
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
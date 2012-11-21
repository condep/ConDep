namespace ConDep.Dsl.Experimental.Application
{
    public class RemoteExecutor : IOfferRemoteExecution
    {
        public IOfferRemoteExecution DosCommand()
        {
            throw new System.NotImplementedException();
        }

        public IOfferRemoteExecution Powershell()
        {
            throw new System.NotImplementedException();
        }
    }
}
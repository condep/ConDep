namespace ConDep.Dsl.Experimental.Application
{
    public interface IOfferRemoteExecution
    {
        IOfferRemoteExecution DosCommand();
        IOfferRemoteExecution Powershell();
    }
}
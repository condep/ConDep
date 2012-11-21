namespace ConDep.Dsl.Experimental.Application
{
    public interface IOfferRemoteOptions
    {
        IOfferRemoteDeployment Deploy { get; }
        IOfferRemoteExecution ExecuteRemote { get; }
        //IOperateLocally FromLocalMachineToServer { get; }
    }
}
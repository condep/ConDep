namespace ConDep.Dsl.Builders
{
    public interface IOfferRemoteOperations
    {
        IOfferRemoteDeployment Deploy { get; }
        IOfferRemoteExecution ExecuteRemote { get; }
        //IOperateLocally FromLocalMachineToServer { get; }
    }
}
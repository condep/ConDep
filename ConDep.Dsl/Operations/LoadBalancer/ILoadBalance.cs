using ConDep.Dsl.SemanticModel;

namespace ConDep.Dsl.Operations.LoadBalancer
{
    public interface ILoadBalance
    {
        void BringOffline(string serverName, LoadBalancerSuspendMethod suspendMethod, IReportStatus status);
        void BringOnline(string serverName, IReportStatus status);
    }
}
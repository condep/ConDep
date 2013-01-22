using ConDep.Dsl.Config;
using ConDep.Dsl.SemanticModel;

namespace ConDep.Dsl.Operations.LoadBalancer
{
    public interface ILoadBalance
    {
        void BringOffline(string serverName, string farm, LoadBalancerSuspendMethod suspendMethod, IReportStatus status);
        void BringOnline(string serverName, string farm, IReportStatus status);
        LbMode Mode { get; set; }
    }
}
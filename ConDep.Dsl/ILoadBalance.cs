using ConDep.Dsl.Config;
using ConDep.Dsl.Operations.LoadBalancer;
using ConDep.Dsl.SemanticModel;

namespace ConDep.Dsl
{
    /// <summary>
    /// Use this interface to implement a custom load balancer
    /// </summary>
    public interface ILoadBalance
    {
        void BringOffline(string serverName, string farm, LoadBalancerSuspendMethod suspendMethod, IReportStatus status);
        void BringOnline(string serverName, string farm, IReportStatus status);
        LbMode Mode { get; set; }
    }
}
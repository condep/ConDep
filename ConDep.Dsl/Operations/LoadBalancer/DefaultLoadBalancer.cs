using ConDep.Dsl.Config;
using ConDep.Dsl.SemanticModel;

namespace ConDep.Dsl.Operations.LoadBalancer
{
    public class DefaultLoadBalancer : ILoadBalance
    {
        public void BringOffline(string serverName, string farm, LoadBalancerSuspendMethod suspendMethod, IReportStatus status)
        {
        }

        public void BringOnline(string serverName, string farm, IReportStatus status)
        {
        }

        public LbMode Mode { get; set; }
    }
}
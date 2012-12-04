using System;
using ConDep.Dsl.Operations.LoadBalancer;
using ConDep.Dsl.SemanticModel;

namespace ConDep.Dsl.LoadBalancer.Ace
{
    public class AceLoadBalancer_Anm_v41 : ILoadBalance
    {
        public void BringOffline(string serverName, LoadBalancerSuspendMethod suspendMethod, IReportStatus status)
        {
            throw new NotImplementedException();
        }

        public void BringOnline(string serverName, IReportStatus status)
        {
            throw new NotImplementedException();
        }
    }
}

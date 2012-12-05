using ConDep.Dsl.LoadBalancer.Ace;
using ConDep.Dsl.Operations.LoadBalancer;
using ConDep.Dsl.SemanticModel;
using NUnit.Framework;

namespace ConDep.Dsl.Tests
{
    [TestFixture]
    public class AceTests
    {
        [Test]
         public void Test()
        {
            var loadBalancer = new AceLoadBalancer_Anm_v41();
            var status = new StatusReporter();
            loadBalancer.BringOffline("", LoadBalancerSuspendMethod.Suspend, status);
        }
    }
}
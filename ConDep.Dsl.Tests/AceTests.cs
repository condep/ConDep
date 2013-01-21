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
        [Ignore]
        public void Test()
        {
            var loadBalancer = new AceLoadBalancer_Anm_v41("104170-ace", "*******");
            var status = new StatusReporter();
            loadBalancer.BringOffline("z63os2swb01-t", "test_env_FARM", LoadBalancerSuspendMethod.Suspend, status);
        }
    }
}
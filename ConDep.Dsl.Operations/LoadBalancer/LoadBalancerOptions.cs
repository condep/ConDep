using ConDep.Dsl.Core;

namespace ConDep.Dsl.Core
{
    public class LoadBalancerOptions
    {
        private readonly WebDeployDefinition _webDeployDefinition;

        public LoadBalancerOptions(WebDeployDefinition webDeployDefinition)
        {
            _webDeployDefinition = webDeployDefinition;
        }
    }
}
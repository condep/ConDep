using System;
using System.Linq;
using System.Reflection;

namespace ConDep.Dsl.Core.LoadBalancer
{
    public class LoadBalancerLookup
    {
        private readonly LoadBalancerSettings _loadBalancerSettings;

        public LoadBalancerLookup(LoadBalancerSettings loadBalancerSettings)
        {
            _loadBalancerSettings = loadBalancerSettings;
        }

        public ILoadBalance GetLoadBalancer()
        {
            if (_loadBalancerSettings != null)
            {
                if(!string.IsNullOrWhiteSpace(_loadBalancerSettings.Provider))
                {
                    var assembly = Assembly.LoadFrom(_loadBalancerSettings.Provider);
                    //var assembly = Assembly.Load(_loadBalancerSettings.Provider);

                    var type = assembly.GetTypes().Where(t => typeof(ILoadBalance).IsAssignableFrom(t)).FirstOrDefault();
                    return Activator.CreateInstance(type, _loadBalancerSettings.Name) as ILoadBalance;
                }
            }
            return new DefaulLoadBalancer();
        }
        
    }
}
using System;
using System.Linq;
using System.Reflection;
using ConDep.Dsl.Model.Config;

namespace ConDep.Dsl.LoadBalancer
{
    public class LoadBalancerLookup
    {
        private readonly LoadBalancerConfig _loadBalancerSettings;

        public LoadBalancerLookup(LoadBalancerConfig loadBalancerSettings)
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
                    return Activator.CreateInstance(type, _loadBalancerSettings) as ILoadBalance;
                }
            }
            return new DefaulLoadBalancer();
        }
        
    }
}
using System;
using System.Linq;
using System.Reflection;
using ConDep.Dsl.Config;

namespace ConDep.Dsl.Operations.LoadBalancer
{
    public interface ILookupLoadBalancer
    {
        ILoadBalance GetLoadBalancer();
    }

    internal class LoadBalancerLookup : ILookupLoadBalancer
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
                    var loadBalancer = Activator.CreateInstance(type, _loadBalancerSettings) as ILoadBalance;
                    loadBalancer.Mode = _loadBalancerSettings.ModeAsEnum;
                    return loadBalancer;
                }
            }
            return new DefaultLoadBalancer();
        }
        
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using ConDep.Dsl.Config;

namespace ConDep.Dsl.SemanticModel
{
    public class ServerInfoHarvester
    {
        private readonly ConDepSettings _settings;
        private List<IHarvestServerInfo> _harvesters;

        public ServerInfoHarvester(ConDepSettings settings)
        {
            _settings = settings;
        }


        public void Harvest(ServerConfig server)
        {
            Harvesters.ForEach(x => x.Harvest(server));
        }

        private List<IHarvestServerInfo> Harvesters
        {
            get
            {
                return _harvesters ?? GetHarvesters(_settings).ToList();
            }
        }

        private IEnumerable<IHarvestServerInfo> GetHarvesters(ConDepSettings settings)
        {
            var internalHarvesters = GetHarvesters(GetType().Assembly.GetTypes());
            if (settings.Options.Assembly == null)
            {
                return internalHarvesters;
            }
            
            var externalHarvesters = GetHarvesters(settings.Options.Assembly.GetTypes());
            return internalHarvesters.Concat(externalHarvesters);
        }

        private IEnumerable<IHarvestServerInfo> GetHarvesters(IEnumerable<Type> types)
        {
            return types.Where(t => t.GetInterfaces().Contains(typeof(IHarvestServerInfo)))
                .Select(t => Activator.CreateInstance(t) as IHarvestServerInfo);
        } 

    }
}
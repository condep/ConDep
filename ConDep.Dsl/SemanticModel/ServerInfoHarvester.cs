using System;
using System.Collections.Generic;
using System.Linq;
using ConDep.Dsl.Config;

namespace ConDep.Dsl.SemanticModel
{
    internal class ServerInfoHarvester
    {
        private readonly ConDepSettings _settings;

        public ServerInfoHarvester(ConDepSettings settings)
        {
            _settings = settings;
        }


        public void Harvest()
        {
            var harvesters = GetHarvesters(_settings).ToList();
            foreach(var server in _settings.Config.Servers)
            {
                harvesters.ForEach(x => x.Harvest(server));
            }
        }

        private IEnumerable<IHarvestServerInfo> GetHarvesters(ConDepSettings settings)
        {
            var internalHarvesters = GetHarvesters(GetType().Assembly.GetTypes());
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
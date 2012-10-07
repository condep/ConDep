using System.Collections.Generic;
using System.Linq;

namespace ConDep.Dsl.Model.Config
{
    public static class Extensions
    {
        public static void RemoveAllExcept(this IList<ServerConfig> servers, string serverName)
        {

            if (servers.All(x => x.Name.ToLower() != serverName))
            {
                throw new ConDepNoServersFoundException(string.Format("Server [{0}] where not one of the servers defined for environment.", serverName));
            }

            var server = servers.Single(x => x.Name.ToLower() == serverName.ToLower());
            servers.Clear();
            servers.Add(server);
        }
    }
}
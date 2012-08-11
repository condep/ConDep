using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using Microsoft.Web.Administration;

namespace ConDep.PowerShell.ApplicationRequestRouting.Infrastructure
{
	public class WebFarmManager
	{
	    private readonly string _farm;
	    private readonly string _server;
	    private readonly bool _useDnsLookup;
	    private IEnumerable<Farm> _farms;
	    private readonly bool _noParams;
	    private IEnumerable<FarmServer> _farmServers;

        private delegate bool WaitCondition(Counters counters);

	    public WebFarmManager()
        {
            _noParams = true;
        }

        public WebFarmManager(string farm, string server, bool useDnsLookup)
        {
            ValidateParams(server, useDnsLookup);

            _farm = farm;
            _server = server;
            _useDnsLookup = useDnsLookup;
        }


	    private static void ValidateParams(string server, bool useDnsLookup)
	    {
	        if (!useDnsLookup) return;

            if (string.IsNullOrEmpty(server))
	        {
	            throw new Exception("When using DNS lookup, Web Farm Server (Name) must be provided.");
	        }

	        if (WildcardPattern.ContainsWildcard(server))
	        {
	            throw new Exception("You can not use DNS lookup together with wildcards (*) in server name.");
	        }
	    }

	    public IEnumerable<Farm> Farms
	    {
	        get { return _farms ?? GetWebFarms(_farm, _server, _useDnsLookup); }
	    }

        public IEnumerable<FarmServer> FarmServers
        {
            get
            {
                if(_farmServers == null)
                {
                    _farmServers = new List<FarmServer>();
                    foreach (var farm in Farms)
                    {
                        ((List<FarmServer>)_farmServers).AddRange(farm.Servers);
                    }
                }
                return _farmServers;
            }
        }

        public IEnumerable<FarmServerStats> SetAvailable(Action<object> writeObject)
        {
            return ChangeState(x => x.MakeServerAvailable(), writeObject);
        }

        public IEnumerable<FarmServerStats> SetUnavailable(bool force, Action<object> writeObject)
        {
            return force ? ChangeState(x => x.MakeServerUnavailable(), writeObject) : ChangeState(x => x.MakeServerUnavailableGracefully(), writeObject);
        }

        public IEnumerable<FarmServerStats> DisallowNewConnections(Action<object> writeObject)
	    {
            return ChangeState(x => x.DisallowNewConnections(), writeObject);
	    }

        public IEnumerable<FarmServerStats> SetHealthy(Action<object> writeObject)
        {
            return ChangeState(x => x.MakeServerHealthy(), writeObject);
        }

        public IEnumerable<FarmServerStats> SetUnhealthy(Action<object> writeObject)
        {
            return ChangeState(x => x.MakeServerUnhealthy(), writeObject);
        }

        public IEnumerable<FarmServerStats> TakeOffline(bool force, Action<object> writeObject)
        {
            if(!force)
            {
                ChangeState(x => x.MakeServerUnavailableGracefully(), null, y => y.CurrentRequests > 0);
            }
            return ChangeState(x => x.TakeServerOffline(), writeObject);
        }

        public IEnumerable<FarmServerStats> TakeOnline(Action<object> writeObject)
        {
            ChangeState(x => x.BringServerOnline());
            return ChangeState(x => x.MakeServerAvailable(), writeObject, y => y.State != FarmServerState.Available);
        }

        private IEnumerable<FarmServerStats> ChangeState(Action<StateExecutor> changeState, Action<object> writeObject = null, WaitCondition waitCondition = null)
        {
            var stats = new List<FarmServerStats>();

            foreach (var farm in Farms)
            {
                foreach (var server in farm.Servers)
                {
                    using (var arr = new ArrServerManager(server.WebFarm, server.Name))
                    {
                        changeState(arr.StateExecutor);
                        arr.Commit();

                        if(waitCondition != null)
                        {
                            while(waitCondition(arr.Counters))
                            {
                                Thread.Sleep(100);
                            }
                        }
                        if(writeObject != null)
                        {
                            writeObject(arr.Counters.GetServerStats());
                        }
                    }
                }
            }
            return stats;
        }

	    private IEnumerable<Farm> GetWebFarms(string farm, string server, bool useDnsLookup)
        {
            if (_noParams)
            {
                return FindAllWebFarms();
            }

            if (!string.IsNullOrEmpty(farm))
            {
                if (!string.IsNullOrEmpty(server))
                {
                    var f = new Farm { Name = farm };
                    f.Servers.Add(new FarmServer { Name = server, WebFarm = farm });

                    return new[] { f };
                }

                return GetWebFarm(farm);
            }

            return !string.IsNullOrEmpty(server) ? GetWebFarmsByServer(server, useDnsLookup) : FindAllWebFarms();
        }

        private static IEnumerable<Farm> GetWebFarm(string farmName)
        {
            using (var serverManager = new ServerManager())
            {
                var config = serverManager.GetApplicationHostConfiguration();
                var webFarmsSection = config.GetSection("webFarms");
                var foundFarmServers = new List<Farm>();

                foreach (var webFarm in webFarmsSection.GetCollection())
                {
                    if (farmName.ToLower() != webFarm["name"].ToString().ToLower()) continue;
                    
                    var farm = new Farm { Name = webFarm["name"].ToString() };
                    foreach (var server in webFarm.GetCollection())
                    {
                        farm.Servers.Add(new FarmServer { WebFarm = farm.Name, Name = server["address"].ToString() });
                    }
                    foundFarmServers.Add(farm);
                }
                return foundFarmServers;
            }
        }


        private static IEnumerable<Farm> FindAllWebFarms()
        {
            using (var serverManager = new ServerManager())
            {
                var config = serverManager.GetApplicationHostConfiguration();
                var webFarmsSection = config.GetSection("webFarms");
                var foundFarmServers = new List<Farm>();

                foreach (var webFarm in webFarmsSection.GetCollection())
                {
                    var farm = new Farm
                                   {
                                       Name = webFarm["name"].ToString()
                                   };
                    foreach (var server in webFarm.GetCollection())
                    {
                        farm.Servers.Add(new FarmServer {WebFarm = farm.Name, Name = server["address"].ToString()});
                    }
                    foundFarmServers.Add(farm);
                }
                return foundFarmServers;
            }
        }

        //Todo: Needs refactoring together with FindAllWebFarms, GetWebFarm and GetWebFarms
        private static IEnumerable<Farm> GetWebFarmsByServer(string serverName, bool useDnsLookup)
        {
            using (var serverManager = new ServerManager())
            {
                var config = serverManager.GetApplicationHostConfiguration();
                var webFarmsSection = config.GetSection("webFarms");

                var foundFarms = new List<Farm>();

                var servers = new List<string>();
                if(useDnsLookup)
                {
                    var ips = Dns.GetHostAddresses(serverName);
                    foreach(var ip in ips)
                    {
                        servers.Add(ip.ToString());
                    }
                }
                else
                {
                    servers.Add(serverName.ToLower());
                }

                foreach (var webFarm in webFarmsSection.GetCollection())
                {
                    foreach (var server in webFarm.GetCollection())
                    {
                        var webFarmServerAddress = server["address"].ToString();

                        if (WildcardPattern.ContainsWildcard(serverName))
                        {
                            var serverAddress = server["address"].ToString();
                            var wildcardPattern = new WildcardPattern(serverName);
                            if(wildcardPattern.IsMatch(serverAddress))
                            {
                                var farm = new Farm { Name = webFarm["name"].ToString() };
                                farm.Servers.Add(new FarmServer
                                {
                                    Name = server["address"].ToString(),
                                    WebFarm = webFarm["name"].ToString()
                                });
                                foundFarms.Add(farm);
                            }
                        }
                        else if (servers.Contains(webFarmServerAddress.ToLower()))
                        {
                            var farm = new Farm {Name = webFarm["name"].ToString()};
                            farm.Servers.Add(new FarmServer
                                                 {
                                                     Name = server["address"].ToString(),
                                                     WebFarm = webFarm["name"].ToString()
                                                 });
                            foundFarms.Add(farm);

                            if(servers.Count == 1)
                            {
                                break;
                            }
                        }
                    }
                }
                return foundFarms;
            }
        }
	}
}
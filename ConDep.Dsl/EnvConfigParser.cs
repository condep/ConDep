using System.IO;
using System.Runtime.Serialization.Json;
using ConDep.Dsl.Model.Config;

namespace ConDep.Dsl
{
    public class EnvConfigParser
    {
        public ConDepConfig GetEnvConfig(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException(string.Format("[{0}] not found.", filePath), filePath);
            }

            using (var fileStream = File.OpenRead(filePath))
            {
                return GetEnvConfig(fileStream);
            }
        }

        public ConDepConfig GetEnvConfig(Stream stream)
        {
            var serializer = new DataContractJsonSerializer(typeof (ConDepConfig));
            var config = (ConDepConfig) serializer.ReadObject(stream);

            foreach (var server in config.Servers)
            {
                if (server.DeploymentUser == null)
                {
                    server.DeploymentUser = config.DeploymentUser;
                }
            }
            return config;
        }
    }

    //public class EnvSettingsParser
    //{
    //    public ConDepEnvironmentSettings Parse(string environment, string json, string explicitServer, bool bypassLb)
    //    {
    //        var envJson = JObject.Parse(json);
    //        return PopulateEnvSettings(environment, envJson, explicitServer, bypassLb);
    //    }

    //    private static ConDepEnvironmentSettings PopulateEnvSettings(string environment, JObject json, string explicitServer, bool bypassLb)
    //    {
    //        bool hasExplicitServerDefined = !string.IsNullOrWhiteSpace(explicitServer);

    //        var envSettings = new ConDepEnvironmentSettings(environment);

    //        if (!bypassLb)
    //        {
    //            PopulateLoadBalancer(envSettings, json);
    //        }

    //        var deploymentUser = json["DeploymentUser"];
    //        if (deploymentUser != null)
    //        {
    //            envSettings.DeploymentUser.UserName = deploymentUser["UserName"].ToString();
    //            envSettings.DeploymentUser.Password = deploymentUser["Password"].ToString();
    //        }
    //        JToken servers;
    //        if (!json.TryGetValue("Servers", out servers))
    //        {
    //            throw new NoServersFoundException("No servers where defined for environment");
    //        }

    //        bool explicitServerFound = false;
    //        foreach (var server in servers)
    //        {
    //            if (hasExplicitServerDefined)
    //            {
    //                if (explicitServer == server["Name"].ToString())
    //                {
    //                    envSettings.Servers.Add(new DeploymentServer(server["Name"].ToString(), envSettings.DeploymentUser));
    //                    explicitServerFound = true;
    //                    break;
    //                }
    //            }
    //            else
    //            {
    //                //todo: how to handle deployment user?? Shouldn't this be on server level??
    //                envSettings.Servers.Add(new DeploymentServer(server["Name"].ToString(), envSettings.DeploymentUser));
    //            }
    //        }

    //        if (hasExplicitServerDefined && !explicitServerFound)
    //        {
    //            throw new NoServersFoundException(string.Format("Server [{0}] where not one of the servers defined for environment.", explicitServer));
    //        }

    //        PopulateCustomSettings(envSettings, json);

    //        return envSettings;
    //    }

    //    private static void PopulateCustomSettings(ConDepEnvironmentSettings envSettings, JObject json)
    //    {
    //        var jsonCustomSettings = json["CustomSettings"];
    //        if (jsonCustomSettings != null)
    //        {
    //            var providers = jsonCustomSettings.ToObject<Dictionary<string, JObject>>();

    //            foreach (var provider in providers)
    //            {
    //                envSettings.CustomSettings.Add(provider.Key, provider.Value.ToObject<IDictionary<string, string>>());
    //            }
    //        }
    //    }

    //    private static void PopulateLoadBalancer(ConDepEnvironmentSettings envSettings, JObject json)
    //    {
    //        if (json["LoadBalancer"] != null)
    //        {
    //            envSettings.LoadBalancer.Name = json["LoadBalancer"]["Name"].ToString();
    //            envSettings.LoadBalancer.Provider = json["LoadBalancer"]["Provider"].ToString();
    //            if (json["LoadBalancer"]["UserName"] != null)
    //            {
    //                envSettings.LoadBalancer.UserName = json["LoadBalancer"]["UserName"].ToString();
    //                envSettings.LoadBalancer.Password = json["LoadBalancer"]["Password"].ToString();
    //            }
    //        }
    //    }
    //}

    //public class WebSiteSettingsParser
    //{
    //    public ConDepEnvironmentSettings Parse(IEnumerable<DeploymentServer> servers, string webSiteJson)
    //    {
    //        var json = JObject.Parse(webSiteJson);
    //        PopulateWebSiteSettings(servers, json);
    //        return null;
    //    }

    //    private static void PopulateWebSiteSettings(IEnumerable<DeploymentServer> servers, JObject json)
    //    {
    //        foreach (JProperty webSite in json["WebSites"].Children())
    //        {
    //            var webSiteName = webSite.Name;

    //            foreach (JProperty server in webSite.Value)
    //            {
    //                AddWebSiteServer(servers, server, webSiteName);
    //            }
    //        }
    //    }

    //    private static void AddWebSiteServer(IEnumerable<DeploymentServer> servers, JProperty server, string webSiteName)
    //    {
    //        var envServer = servers.Single(x => x.ServerName == server.Name);
    //        var envWebSite = new ConDepWebSiteSettings(webSiteName);

    //        foreach (var binding in server.Value)
    //        {
    //            envWebSite.Bindings.Add(CreateWebSiteBinding(binding));
    //        }
    //        envServer.WebSites.Add(envWebSite);
    //    }

    //    private static ConDepWebSiteBinding CreateWebSiteBinding(JToken binding)
    //    {
    //        var bindingType = binding["BindingType"].ToString();
    //        var port = binding["Port"].ToString();
    //        var ip = binding["Ip"].ToString();
    //        var hostHeader = binding["HostHeader"].ToString();
    //        return new ConDepWebSiteBinding(bindingType, port, ip, hostHeader);
    //    }

    //}
}
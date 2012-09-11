using System.IO;
using System.Linq;
using ConDep.Dsl;
using Newtonsoft.Json.Linq;

namespace ConDep.Console
{
    internal class JsonConfigParser
    {
        private readonly string _configDir;
        private readonly string _environment;

        public JsonConfigParser(string configDir, string environment)
        {
            _configDir = configDir;
            _environment = environment;
        }

        public ConDepEnvironmentSettings GetEnvSettings()
        {
            var envFileName = string.Format("{0}.Env.js", _environment);
            var envFilePath = Path.Combine(_configDir, envFileName);
            if (!File.Exists(envFilePath))
            {
                throw new FileNotFoundException(string.Format("[{0}] not found.", envFilePath), envFilePath);
            }
            
            var webSitesFileName = string.Format("WebSites.{0}.Env.js", _environment);
            var webSitesFilePath = Path.Combine(_configDir, webSitesFileName);

            var envJsonText = File.ReadAllText(Path.Combine(_configDir, string.Format("{0}.Env.js", _environment)));

            var envJson = JObject.Parse(envJsonText);
            var envSettings = PopulateEnvSettings(_environment, envJson);

            if (File.Exists(webSitesFilePath))
            {
                var webSiteJsonText = File.ReadAllText(Path.Combine(_configDir, string.Format("WebSites.{0}.Env.js", _environment)));
                var webSiteJson = JObject.Parse(webSiteJsonText);
                PopulateWebSiteSettings(envSettings, webSiteJson);
            }

            return envSettings;
        }

        private static ConDepEnvironmentSettings PopulateEnvSettings(string environment, JObject json)
        {
            var envSettings = new ConDepEnvironmentSettings(environment);

            PopulateLoadBalancer(envSettings, json);

            var deploymentUser = json["DeploymentUser"];
            if (deploymentUser != null)
            {
                envSettings.DeploymentUser.UserName = deploymentUser["UserName"].ToString();
                envSettings.DeploymentUser.Password = deploymentUser["Password"].ToString();
            }

            foreach (var server in json["Servers"])
            {
                //todo: how to handle deployment user?? Shouldn't this be on server level??
                envSettings.Servers.Add(new DeploymentServer(server["Name"].ToString(), envSettings.DeploymentUser));
            }

            return envSettings;
        }

        private static void PopulateLoadBalancer(ConDepEnvironmentSettings envSettings, JObject json)
        {
            if (json["LoadBalancer"] != null)
            {
                envSettings.LoadBalancer.Name = json["LoadBalancer"]["Name"].ToString();
                envSettings.LoadBalancer.Provider = json["LoadBalancer"]["Provider"].ToString();
                if (json["LoadBalancer"]["UserName"] != null)
                {
                    envSettings.LoadBalancer.UserName = json["LoadBalancer"]["UserName"].ToString();
                    envSettings.LoadBalancer.Password = json["LoadBalancer"]["Password"].ToString();
                }
            }
        }

        //Todo: Must refactor!
        private static void PopulateWebSiteSettings(ConDepEnvironmentSettings envSettings, JObject json)
        {
            foreach (JProperty webSite in json["WebSites"].Children())
            {
                var webSiteName = webSite.Name;

                foreach (JProperty server in webSite.Value)
                {
                    AddWebSiteServer(envSettings, server, webSiteName);
                }
            }
        }

        private static void AddWebSiteServer(ConDepEnvironmentSettings envSettings, JProperty server, string webSiteName)
        {
            var envServer = envSettings.Servers.Where(x => x.ServerName == server.Name).FirstOrDefault();
            var envWebSite = new ConDepWebSiteSettings(webSiteName);

            foreach (var binding in server.Value)
            {
                envWebSite.Bindings.Add(CreateWebSiteBinding(binding));
            }
            envServer.WebSites.Add(envWebSite);
        }

        private static ConDepWebSiteBinding CreateWebSiteBinding(JToken binding)
        {
            var bindingType = binding["BindingType"].ToString();
            var port = binding["Port"].ToString();
            var ip = binding["Ip"].ToString();
            var hostHeader = binding["HostHeader"].ToString();
            return new ConDepWebSiteBinding(bindingType, port, ip, hostHeader);
        }

    }
}
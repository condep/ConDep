using System.IO;
using System.Linq;
using System.Reflection;
using ConDep.Dsl.Core;
using Newtonsoft.Json.Linq;

namespace ConDep.Console
{
    class Program
    {
        //ConDep.exe MyAssembly.dll Env=Test [Server=web01] [Applications=Selvbetjent] [/InfraOnly] [/DeployOnly]
        //
        //If only assembly and Env is provided, then ConDep will deploy all applications to all servers utilizing load balancer if provided
        //If assembly, Env and Server is provided, then ConDep will take Server offline from Load Balancer if provided and deploy all applications
        //If assembly, Env, Server and Application is provided, then ConDep will take Server offline from Load Balancer if provided and deploy only the Applications specified
        static void Main(string[] args)
        {
            var optionHandler = new CommandLineOptionHandler(args);

            var assembly = FindAssembly(optionHandler.Params.AssemblyName);
            var type = assembly.GetTypes().Where(t => typeof(ConDepConfigurator).IsAssignableFrom(t)).FirstOrDefault();

            var executionPath = Path.GetDirectoryName(type.Assembly.Location);
            var envFileName = string.Format("{0}.Env.js", optionHandler.Params.Environment);
            var envFilePath = Path.Combine(executionPath, envFileName);
            var webSitesFileName = string.Format("WebSites.{0}.Env.js", optionHandler.Params.Environment);
            var webSitesFilePath = Path.Combine(executionPath, webSitesFileName);

            if(!File.Exists(envFilePath))
            {
                throw new FileNotFoundException("Not found.", envFilePath);
            }
            var envJsonText = File.ReadAllText(Path.Combine(Path.GetDirectoryName(type.Assembly.Location), string.Format("{0}.Env.js", optionHandler.Params.Environment)));

            var envJson = JObject.Parse(envJsonText);
            var envSettings = PopulateEnvSettings(optionHandler.Params.Environment, envJson);

            if (File.Exists(webSitesFilePath))
            {
                var webSiteJsonText = File.ReadAllText(Path.Combine(Path.GetDirectoryName(type.Assembly.Location), string.Format("WebSites.{0}.Env.js", optionHandler.Params.Environment)));
                var webSiteJson = JObject.Parse(webSiteJsonText);
                PopulateWebSiteSettings(envSettings, webSiteJson);
            }

            Executor.ExecuteFromAssembly(assembly, envSettings, optionHandler.Params.TraceLevel);
        }

        private static void PopulateWebSiteSettings(ConDepEnvironmentSettings envSettings, JObject json)
        {
            foreach(JProperty webSite in json["WebSites"].Children())
            {
                var webSiteName = webSite.Name;

                foreach(JProperty server in webSite.Value)
                {
                    var envServer = envSettings.Servers.Where(x => x.ServerName == server.Name).FirstOrDefault();
                    var envWebSite = new ConDepWebSiteSettings(webSiteName);

                    foreach(var binding in server.Value)
                    {
                        var bindingType = binding["BindingType"].ToString();
                        var port = binding["Port"].ToString();
                        var ip = binding["Ip"].ToString();
                        var hostHeader = binding["HostHeader"].ToString();

                        envWebSite.Bindings.Add(new ConDepWebSiteBinding(bindingType, port, ip, hostHeader));
                    }

                    envServer.WebSites.Add(envWebSite);
                }
            }
        }

        private static ConDepEnvironmentSettings PopulateEnvSettings(string environment, JObject json)
        {
            var envSettings = new ConDepEnvironmentSettings(environment);

            PopulateLoadBalancer(envSettings, json);
            foreach(var server in json["Servers"])
            {
                envSettings.Servers.Add(new DeploymentServer(server["Name"].ToString()));
            }

            var deploymentUser = json["DeploymentUser"];
            if(deploymentUser != null)
            {
                envSettings.DeploymentUser.UserName = deploymentUser["UserName"].ToString();
                envSettings.DeploymentUser.Password = deploymentUser["Password"].ToString();
            }
            return envSettings;
        }

        private static void PopulateLoadBalancer(ConDepEnvironmentSettings envSettings, JObject json)
        {
            if(json["LoadBalancer"] != null)
            {
                envSettings.LoadBalancer.Name = json["LoadBalancer"]["Name"].ToString();
                envSettings.LoadBalancer.Provider = json["LoadBalancer"]["Provider"].ToString();
                if(json["LoadBalancer"]["UserName"] != null)
                {
                    envSettings.LoadBalancer.UserName = json["LoadBalancer"]["UserName"].ToString();
                    envSettings.LoadBalancer.Password = json["LoadBalancer"]["UserName"].ToString();
                }
            }
        }

        private static Assembly FindAssembly(string assemblyName)
        {
            var assemblyFileName = Path.GetFullPath(assemblyName);
            return Assembly.LoadFile(assemblyFileName);
        }
    }
}

using System.IO;
using System.Linq;
using System.Reflection;
using ConDep.Dsl.Core;
using Newtonsoft.Json.Linq;

namespace ConDep.Console
{
    class Program
    {
        //Blue/Green deployment - with possibillity to roll back to blue/green

        //ConDep.exe MyAssembly.dll Env=Test [Server=web01] [Applications=Selvbetjent] [/InfraOnly] [/DeployOnly]
        //
        //If only assembly and Env is provided, then ConDep will deploy all applications to all servers utilizing load balancer if provided
        //If assembly, Env and Server is provided, then ConDep will take Server offline from Load Balancer if provided and deploy all applications
        //If assembly, Env, Server and Application is provided, then ConDep will take Server offline from Load Balancer if provided and deploy only the Applications specified
        static void Main(string[] args)
        {
            if(args.Length == 0)
            {
                PrintHelp();
            }

            var assembly = FindAssembly(args);
            var type = assembly.GetTypes().Where(t => typeof(ConDepConfigurator).IsAssignableFrom(t)).FirstOrDefault();

            var envJsonText = File.ReadAllText(Path.Combine(Path.GetDirectoryName(type.Assembly.Location), "Dev.Env.js"));
            var webSiteJsonText = File.ReadAllText(Path.Combine(Path.GetDirectoryName(type.Assembly.Location), "WebSites.Dev.Env.js"));

            var envJson = JObject.Parse(envJsonText);
            var webSiteJson = JObject.Parse(webSiteJsonText);

            var envSettings = PopulateEnvSettings(envJson);
            PopulateWebSiteSettings(envSettings, webSiteJson);

            Executor.ExecuteFromAssembly(assembly, envSettings);
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

        private static ConDepEnvironmentSettings PopulateEnvSettings(JObject json)
        {
            var envSettings = new ConDepEnvironmentSettings();
            
            PopulateLoadBalancer(envSettings, json);
            foreach(var server in json["Servers"])
            {
                envSettings.Servers.Add(new DeploymentServer(server["Name"].ToString()));
            }
            if(envSettings.DeploymentUser.IsDefined)
            {
                envSettings.DeploymentUser.UserName = json["DeploymentUser"]["UserName"].ToString();
                envSettings.DeploymentUser.Password = json["DeploymentUser"]["Password"].ToString();
            }
            return envSettings;
        }

        private static void PopulateLoadBalancer(ConDepEnvironmentSettings envSettings, JObject json)
        {
            if(json["LoadBalancer"] != null)
            {
                envSettings.LoadBalancer.Name = json["LoadBalancer"]["Name"].ToString();
                envSettings.LoadBalancer.Provider = json["LoadBalancer"]["Provider"].ToString();
            }
        }

        private static Assembly FindAssembly(string[] args)
        {
            var assemblyName = args[0];
            //var currentPath = AppDomain.CurrentDomain.BaseDirectory;


            var assemblyFileName = assemblyName;//Path.Combine(currentPath, assemblyName);
            return Assembly.LoadFile(assemblyFileName);
        }

        private static void PrintHelp()
        {
            System.Console.WriteLine("usage: ConDep <assembly> Env=<environment> [Server=<server>]\n" +
                                     "              [Applications=<app1>,<app2>,...>]");
        }
    }
}

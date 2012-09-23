using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using ConDep.Dsl;
using ConDep.Dsl.WebDeploy;

namespace ConDep.Console
{
    sealed class Program
    {
        //ConDep.exe MyAssembly.dll Env=Test [Server=web01] [Applications=Selvbetjent] [/InfraOnly] [/DeployOnly]
        //
        //If only assembly and Env is provided, then ConDep will deploy all applications to all servers utilizing load balancer if provided
        //If assembly, Env and Server is provided, then ConDep will take Server offline from Load Balancer if provided and deploy all applications
        //If assembly, Env, Server and Application is provided, then ConDep will take Server offline from Load Balancer if provided and deploy only the Applications specified
        static void Main(string[] args)
        {
            var exitCode = 0;
            try
            {
                var optionHandler = new CommandLineOptionHandler(args);
                var configAssemblyLoader = new ConfigurationAssemblyHandler(optionHandler.Params.AssemblyName);
                var assembly = configAssemblyLoader.GetConfigAssembly();

                var jsonConfigParser = new JsonConfigParser(Path.GetDirectoryName(assembly.Location), optionHandler.Params.Environment);
                var envSettings = jsonConfigParser.GetEnvSettings(optionHandler.Params.Server, optionHandler.Params.BypassLB);
                var conDepOptions = new ConDepOptions(optionHandler.Params.Context, optionHandler.Params.DeployOnly, optionHandler.Params.InfraOnly, optionHandler.Params.TraceLevel, optionHandler.Params.PrintSequence);

                var status = new WebDeploymentStatus();
                ConDepConfigurationExecutor.ExecuteFromAssembly(assembly, envSettings, conDepOptions, status);

                if(status.HasErrors)
                {
                    exitCode = 1;
                }
            }
            catch
            {
                exitCode = 1;
                throw;
            }
            finally
            {
                Environment.Exit(exitCode);
            }
        }
    }
}

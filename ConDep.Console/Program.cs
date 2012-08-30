using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using ConDep.Dsl;

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
            //System.Console.ReadLine();
            var optionHandler = new CommandLineOptionHandler(args);
            var configAssemblyLoader = new ConfigurationAssemblyHandler(optionHandler.Params.AssemblyName);
            var assembly = configAssemblyLoader.GetConfigAssembly();

            var jsonConfigParser = new JsonConfigParser(Path.GetDirectoryName(assembly.Location), optionHandler.Params.Environment);
            var envSettings = jsonConfigParser.GetEnvSettings();
            var conDepOptions = new ConDepOptions(optionHandler.Params.Context, optionHandler.Params.DeployOnly, optionHandler.Params.InfraOnly, optionHandler.Params.TraceLevel, optionHandler.Params.PrintSequence);
            ConDepConfigurationExecutor.ExecuteFromAssembly(assembly, envSettings, conDepOptions);
        }
    }
}

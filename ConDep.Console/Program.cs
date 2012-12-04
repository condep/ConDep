using System;
using System.IO;
using System.Reflection;
using ConDep.Dsl;
using ConDep.Dsl.Config;
using ConDep.Dsl.Logging;
using ConDep.Dsl.SemanticModel;
using ConDep.Dsl.WebDeploy;

namespace ConDep.Console
{
    sealed internal class Program
    {
        static void Main(string[] args)
        {
            var exitCode = 0;
            try
            {
                new LogConfigLoader().Load();
                Logger.LogSectionStart("ConDep");

                var optionHandler = new CommandLineOptionHandler(args);
                var configAssemblyLoader = new ConDepAssemblyHandler(optionHandler.Params.AssemblyName);
                var assembly = configAssemblyLoader.GetConfigAssembly();

                var conDepOptions = new ConDepOptions(optionHandler.Params.Context, optionHandler.Params.DeployOnly, optionHandler.Params.InfraOnly);
                var envSettings = GetEnvConfig(optionHandler.Params, assembly);

                var status = new WebDeploymentStatus();
                ConDepConfigurationExecutor.ExecuteFromAssembly(assembly, envSettings, conDepOptions, status);

                if(status.HasErrors)
                {
                    exitCode = 1;
                }
                else
                {
                    status.PrintSummery();
                }
            }
            catch
            {
                exitCode = 1;
                throw;
            }
            finally
            {
                Logger.LogSectionEnd("ConDep");
                Environment.Exit(exitCode);
            }
        }

        private static ConDepConfig GetEnvConfig(CommandLineParams cmdParams, Assembly assembly)
        {
            var envFileName = string.Format("{0}.Env.json", cmdParams.Environment);
            var envFilePath = Path.Combine(Path.GetDirectoryName(assembly.Location), envFileName);

            var jsonConfigParser = new EnvConfigParser();
            var envConfig = jsonConfigParser.GetEnvConfig(envFilePath);
            envConfig.EnvironmentName = cmdParams.Environment;

            //todo: add unit tests for these conditions
            if (!string.IsNullOrWhiteSpace(cmdParams.Server))
            {
                envConfig.Servers.RemoveAllExcept(cmdParams.Server);
            }

            if (cmdParams.BypassLB)
            {
                envConfig.LoadBalancer = null;
            }
            return envConfig;
        }
    }
}

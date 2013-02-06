using System;
using System.IO;
using System.Net;
using System.Reflection;
using System.Threading;
using ConDep.Dsl;
using ConDep.Dsl.Config;
using ConDep.Dsl.Logging;
using ConDep.Dsl.Operations;
using ConDep.Dsl.SemanticModel;
using ConDep.Dsl.SemanticModel.WebDeploy;
using ConDep.WebQ.Client;
using ConDep.WebQ.Data;

namespace ConDep.Console
{
    sealed internal class Program
    {
        static void Main(string[] args)
        {
            var exitCode = 0;
            WebQHandler webQHandler = null;
            try
            {
                new LogConfigLoader().Load();

                var optionHandler = new CommandLineOptionHandler(args);

                if (optionHandler.Params.InstallWebQ)
                {
                    throw new NotImplementedException();
                }
                else
                {
                    Logger.LogSectionStart("ConDep");
                    if (!string.IsNullOrWhiteSpace(optionHandler.Params.WebQAddress))
                    {
                        webQHandler = new WebQHandler(optionHandler.Params.WebQAddress, optionHandler.Params.Environment);
                        webQHandler.WaitInQueue();
                    }

                    var configAssemblyLoader = new ConDepAssemblyHandler(optionHandler.Params.AssemblyName);
                    var assembly = configAssemblyLoader.GetAssembly();

                    var conDepOptions = new ConDepOptions(optionHandler.Params.DeployAllApps,
                                                          optionHandler.Params.Application,
                                                          optionHandler.Params.DeployOnly,
                                                          optionHandler.Params.WebDeployExist,
                                                          optionHandler.Params.StopAfterMarkedServer,
                                                          optionHandler.Params.ContinueAfterMarkedServer,
                                                          assembly);
                    var envSettings = GetEnvConfig(optionHandler.Params, assembly);

                    var status = new WebDeploymentStatus();
                    ConDepConfigurationExecutor.ExecuteFromAssembly(assembly, envSettings, conDepOptions, status);

                    if (status.HasErrors)
                    {
                        exitCode = 1;
                    }
                    else
                    {
                        status.EndTime = DateTime.Now;
                        status.PrintSummary();
                    }
                }
            }
            catch (Exception ex)
            {
                exitCode = 1;
                Logger.Error("ConDep reported a fatal error:");
                Logger.Error("Message: " + ex.Message);
                Logger.Error("Stack trace:\n" + ex.StackTrace);
            }
            finally
            {
                if(webQHandler != null)
                {
                    webQHandler.LeaveQueue();
                }

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

            if (cmdParams.BypassLB)
            {
                envConfig.LoadBalancer = null;
            }
            return envConfig;
        }
    }

    internal class WebQHandler
    {
        private readonly string _environment;
        private Client _client;
        private WebQItem _item;

        public WebQHandler(string webQAddress, string environment)
        {
            _client = new Client(new Uri(webQAddress));
            _environment = environment;
        }

        public void WaitInQueue()
        {
            Logger.LogSectionStart("Waiting in queue");
            try
            {
                _item = _client.Enqueue(_environment);
                var currentPosition = _item.Position;
                if (currentPosition == 0)
                {
                    _client.SetAsStarted(_item);
                    return;
                }

                var timeout = 3*60;
                var waitTime = 0;
                do
                {
                    Thread.Sleep(10000);
                    waitTime += 3;
                    _item = _client.Peek(_item);

                    if(currentPosition != _item.Position)
                    {
                        Logger.Info(
                            string.Format("Waiting in deployment queue. There are {0} deployment(s) waiting to finish...",
                                          _item.Position));
                        currentPosition = _item.Position;
                    }
                } while (_item.Position != 0 || waitTime > timeout);

                _item = _client.SetAsStarted(_item);

                if (waitTime >= timeout)
                {
                    _client.Dequeue(_item);
                    throw new TimeoutException("ConDep timed out waiting in queue.");
                }
            }
            finally
            {
                Logger.LogSectionEnd("Waiting in queue");
            }
        }

        public void LeaveQueue()
        {
            _client.Dequeue(_item);
        }
    }
}

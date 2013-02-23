using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using ConDep.Dsl.Config;
using ConDep.Dsl.Logging;
using ConDep.Dsl.SemanticModel;
using ConDep.Dsl.SemanticModel.WebDeploy;
using ConDep.WebQ.Client;

namespace ConDep.Console
{
    sealed internal class Program
    {
        static void Main(string[] args)
        {
            var exitCode = 0;
            WebQueue webQ = null;
            try
            {
                new LogConfigLoader().Load();
                Logger.TraceLevel = TraceLevel.Info;

                var optionHandler = new CommandLineOptionHandler(args);
                if (optionHandler.Params.InstallWebQ)
                {
                    throw new NotImplementedException();
                }
                else
                {
                    PrintCopyrightMessage();
                    Logger.LogSectionStart("ConDep");
                    if (!string.IsNullOrWhiteSpace(optionHandler.Params.WebQAddress))
                    {
                        webQ = new WebQueue(optionHandler.Params.WebQAddress, optionHandler.Params.Environment);
                        webQ.WebQueuePositionUpdate += (sender, eventArgs) => Logger.Info(eventArgs.Message);
                        webQ.WebQueueTimeoutUpdate += (sender, eventArgs) => Logger.Info(eventArgs.Message);
                        Logger.LogSectionStart("Waiting in Deployment Queue");
                        try
                        {
                            webQ.WaitInQueue(TimeSpan.FromMinutes(30));
                        }
                        finally
                        {
                            Logger.LogSectionEnd("Waiting in Deployment Queue");
                        }
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
                    var envSettings = ConfigHandler.GetEnvConfig(optionHandler.Params.Environment, optionHandler.Params.BypassLB, assembly);

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
                if(webQ != null)
                {
                    webQ.LeaveQueue();
                }

                Logger.LogSectionEnd("ConDep");
                Environment.Exit(exitCode);
            }
        }

        private static void PrintCopyrightMessage()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var versionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);

            Logger.Info(string.Format("ConDep Version {0}", versionInfo.ProductVersion.Substring(0, versionInfo.ProductVersion.LastIndexOf("."))));
            Logger.Info("Copyright (c) Jon Arild Torresdal");
        }
    }
}

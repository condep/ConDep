using System;
using System.Diagnostics;
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

                var conDepSettings = new ConDepSettings();
                CommandLineOptionHandler.ParseArgs(args, conDepSettings.Options);

                if (conDepSettings.Options.InstallWebQ)
                {
                    throw new NotImplementedException();
                }
                else
                {
                    PrintCopyrightMessage();
                    Logger.LogSectionStart("ConDep");
                    if (!string.IsNullOrWhiteSpace(conDepSettings.Options.WebQAddress))
                    {
                        webQ = new WebQueue(conDepSettings.Options.WebQAddress, conDepSettings.Options.Environment);
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

                    var configAssemblyLoader = new ConDepAssemblyHandler(conDepSettings.Options.AssemblyName);
                    conDepSettings.Options.Assembly = configAssemblyLoader.GetAssembly();

                    conDepSettings.Config = ConfigHandler.GetEnvConfig(conDepSettings);

                    var status = new WebDeploymentStatus();
                    ConDepConfigurationExecutor.ExecuteFromAssembly(conDepSettings, status);

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
                Logger.Verbose("Stack trace:\n" + ex.StackTrace);
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

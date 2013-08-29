using System;
using System.Diagnostics;
using ConDep.Dsl.Logging;

namespace ConDep.Console
{
    sealed internal class Program
    {
        static void Main(string[] args)
        {
            var exitCode = 0;
            AppDomain.CurrentDomain.ProcessExit += OnExit;

            try
            {
                ConfigureLogger();
                ExecuteCommand(args);
            }
            catch (Exception ex)
            {
                exitCode = 1;
                Logger.Error("ConDep reported a fatal error:");
                Logger.Error("Message: " + ex.Message);
                Logger.Verbose("Stack trace:\n" + ex.StackTrace);
            }
        }

        private static void ConfigureLogger()
        {
            new LogConfigLoader().Load();
            Logger.TraceLevel = TraceLevel.Info;
        }

        private static void ExecuteCommand(string[] args)
        {
            var helpWriter = new CmdHelpWriter(System.Console.Out);

            try
            {
                var handler = CmdFactory.Resolve(args);
                handler.Execute(helpWriter, Logger.LogInstance);
            }
            catch (Exception ex)
            {
                //if (handler != null)
                //{
                //    //handler.WriteHelp();
                //    System.Console.ForegroundColor = ConsoleColor.Red;
                //    helpWriter.WriteException(ex);
                //    System.Console.ResetColor();
                //}
                //else
                //{
                System.Console.ForegroundColor = ConsoleColor.Red;
                helpWriter.WriteException(ex);
                System.Console.ResetColor();
                System.Console.WriteLine("For help type ConDep Help <command>");
                //}
                Environment.Exit(1);
            }
        }

        static void OnExit(object sender, EventArgs e)
        {
            
        }
    }
}

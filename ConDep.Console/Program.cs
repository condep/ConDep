using System;
using System.Diagnostics;
using ConDep.Console.Deploy;
using ConDep.Dsl.Logging;

namespace ConDep.Console
{
    sealed internal class Program
    {
        private static IHandleConDepCommands _handler;

        static void Main(string[] args)
        {
            var exitCode = 0;
            AppDomain.CurrentDomain.ProcessExit += Console_OnExit;
            System.Console.CancelKeyPress += Console_CancelKeyPress;

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
            Environment.ExitCode = exitCode;
        }

        private static void Console_CancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            Logger.Warn("I'm exiting now because you force me!");
            if (_handler != null)
            {
                Logger.Warn("Cancelling handler!");
                _handler.Cancel();
            }
        }

        private static void ConfigureLogger()
        {
            new LogConfigLoader().Load();
            new Logger().AutoResolveLogger();
            Logger.TraceLevel = TraceLevel.Info;
        }

        private static void ExecuteCommand(string[] args)
        {
            var helpWriter = new CmdHelpWriter(System.Console.Out);

            try
            {
                _handler = CmdFactory.Resolve(args);
                _handler.Execute(helpWriter, Logger.LogInstance);
            }
            catch (AggregateException aggEx)
            {
                var flattenEx = aggEx.Flatten();
                foreach (var ex in flattenEx.InnerExceptions)
                {
                    System.Console.ForegroundColor = ConsoleColor.Red;
                    helpWriter.WriteException(ex);
                    System.Console.ResetColor();
                    System.Console.WriteLine("For help type ConDep Help <command>");
                }
                Environment.Exit(1);
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

        static void Console_OnExit(object sender, EventArgs e)
        {
            Logger.Info("I'm exiting now!");   
        }
    }
}

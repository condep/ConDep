using System;
using System.Diagnostics;
using ConDep.Dsl;
using ConDep.Dsl.Logging;
using NDesk.Options;

namespace ConDep.Console
{
    internal sealed class CommandLineOptionHandler
    {
        private readonly string[] _args;
        private CommandLineParams _params;
        private bool _initialized;
        private HelpWriter _helpWriter;

        public CommandLineOptionHandler(string[] args)
        {
            _args = args;
            _helpWriter = new HelpWriter(System.Console.Out);
        }

        public CommandLineParams Params
        {
            get
            {
                if(!_initialized)
                {
                    _initialized = true;
                    Init();
                }
                return _params;
            }
            set { _params = value; }
        }

        private void Init()
        {
            var optionSet = GetOptionSet();

            ValidateParams(optionSet);
        }

        private void ValidateParams(OptionSet optionSet)
        {
            ValidateNumberOfParams(optionSet);
            CheckForAndShowHelp(optionSet);
            ValidateAssemblyName(optionSet);
            ValidateEnvironment(optionSet);
            ValidateApplication(optionSet);
        }

        private void ValidateNumberOfParams(OptionSet optionSet)
        {
            if (_args.Length >= 3) return;

            _helpWriter.PrintHelp(optionSet);
            Environment.Exit(1);
        }

        private void ValidateEnvironment(OptionSet optionSet)
        {
            Params.Environment = _args[1];
            if (string.IsNullOrWhiteSpace(Params.Environment))
            {
                ExitWithMessage(optionSet, "No environment provided.");
            }
        }

        private void ValidateAssemblyName(OptionSet optionSet)
        {
            Params.AssemblyName = _args[0];
            if (string.IsNullOrWhiteSpace(Params.AssemblyName))
            {
                ExitWithMessage(optionSet, "No assembly provided.");
            }
        }

        private void ValidateApplication(OptionSet optionSet)
        {
            var appParam = _args[2];

            if (string.IsNullOrWhiteSpace(appParam))
            {
                ExitWithMessage(optionSet, "No application provided.");
            }

            Params.DeployAllApps = appParam.ToLower() == "--deployallapps";
            if(!Params.DeployAllApps)
            {
                Params.Application = appParam;
            }
        }

        private void CheckForAndShowHelp(OptionSet optionSet)
        {
            if (!Params.ShowHelp) return;

            _helpWriter.PrintHelp(optionSet);
            Environment.Exit(0);
        }

        private void ExitWithMessage(OptionSet optionSet, string message)
        {
            System.Console.Error.WriteLine(message);
            System.Console.Error.WriteLine();
            _helpWriter.PrintHelp(optionSet);
            Environment.Exit(1);
        }

        private OptionSet GetOptionSet()
        {
            Params = new CommandLineParams(); 

            var optionSet = new OptionSet()
                                {
                                    {"t=|traceLevel=", "The level of verbosity on output. Valid values are Off, Info, Warning, Error, Verbose. Default is Info.\n", v =>
                                        {
                                            var traceLevel = ConvertStringToTraceLevel(v);
                                            Logger.TraceLevel = traceLevel;
                                            Params.TraceLevel = traceLevel;
                                        }},
                                    {"w|webDeployExist", "Tells ConDep not to deploy WebDeploy, because it already exist on server.\n", v => Params.WebDeployExist = v != null },
                                    {"q=|webQ=", "Will use ConDep's Web Queue to queue the deployment, preventing multiple deployments to execute at the same time. Useful when ConDep is triggered often from CI environments. Expects the url for the WebQ as its value.\n", v => Params.WebQAddress = v },
                                    {"i|infraOnly", "Deploy infrastructure only\n", v => Params.InfraOnly = v != null },
                                    {"d|deployOnly", "Deploy all except infrastructure\n", v => Params.DeployOnly = v != null},
                                    {"b|bypassLB", "Don't use configured load balancer during execution.\n", v => Params.BypassLB = v != null},
                                    {"s|sams|stopAfterMarkedServer", "Will only deploy to server marked as StopServer in json config, or first server if no server is marked. After execution, run ConDep with the continueAfterMarkedServer switch to continue deployment to remaining servers.\n", v => Params.StopAfterMarkedServer = v != null},
                                    {"c|cams|continueAfterMarkedServer", "Will continue deployment to remaining servers. Used after ConDep has previously executed with the stopAfterMarkedServer switch.\n", v => Params.ContinueAfterMarkedServer = v != null},
                                    {"h|?|help",  "show this message and exit", v => Params.ShowHelp = v != null }
                                };
            try
            {
                optionSet.Parse(_args);
            }
            catch (OptionException oe)
            {
                System.Console.Error.WriteLine(oe.ToString());
                Environment.Exit(1);
            }
            return optionSet;
        }

        private static TraceLevel ConvertStringToTraceLevel(string traceLevel)
        {
            if (traceLevel == null) return TraceLevel.Info;

            switch (traceLevel.ToLower())
            {
                case "off": return TraceLevel.Off;
                case "warning": return TraceLevel.Warning;
                case "error": return TraceLevel.Error;
                case "verbose": return TraceLevel.Verbose;
                default: return TraceLevel.Info;
            }
        }
    }
}
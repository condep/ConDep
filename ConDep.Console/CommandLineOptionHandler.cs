using System;
using System.Diagnostics;
using ConDep.Dsl.Config;
using ConDep.Dsl.Logging;
using NDesk.Options;

namespace ConDep.Console
{
    internal sealed class CommandLineOptionHandler
    {
        private readonly string[] _args;
        private readonly ConDepOptions _options;
        private readonly HelpWriter _helpWriter;

        private CommandLineOptionHandler(string[] args, ConDepOptions options)
        {
            _args = args;
            _options = options;
            _helpWriter = new HelpWriter(System.Console.Out);
        }

        public static void ParseArgs(string[] args, ConDepOptions options)
        {
            var optHandler = new CommandLineOptionHandler(args, options);
            optHandler.Init();
        }

        private void Init()
        {
            var optionSet = GetOptionSet();

            ValidateParams(optionSet);
        }

        private void ValidateParams(OptionSet optionSet)
        {
            if(WebQInstallParamExist()) return;

            ValidateNumberOfParams(optionSet);
            CheckForAndShowHelp(optionSet);
            ValidateAssemblyName(optionSet);
            ValidateEnvironment(optionSet);
            ValidateApplication(optionSet);
        }

        private bool WebQInstallParamExist()
        {
            if(!(_args.Length >= 1 && _args.Length <= 2))
            {
                return false;
            }

            var installWebQParam = _args[0].ToLower();
            if (installWebQParam == "--installWebQ" || installWebQParam == "-installWebQ" || installWebQParam == "/installWebQ")
            {
                _options.InstallWebQ = true;
            }
            else
            {
                return false;
            }

            if(_args.Length == 2)
            {
                _options.InstallWebQOnServer = _args[1];
            }
            return _options.InstallWebQ;
        }

        private void ValidateNumberOfParams(OptionSet optionSet)
        {
            if (_args.Length >= 3) return;

            _helpWriter.PrintHelp(optionSet);
            Environment.Exit(1);
        }

        private void ValidateEnvironment(OptionSet optionSet)
        {
            _options.Environment = _args[1];
            if (string.IsNullOrWhiteSpace(_options.Environment))
            {
                ExitWithMessage(optionSet, "No environment provided.");
            }
        }

        private void ValidateAssemblyName(OptionSet optionSet)
        {
            _options.AssemblyName = _args[0];
            if (string.IsNullOrWhiteSpace(_options.AssemblyName))
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

            _options.DeployAllApps = appParam.ToLower() == "--deployallapps";
            if(!_options.DeployAllApps)
            {
                _options.Application = appParam;
            }
        }

        private void CheckForAndShowHelp(OptionSet optionSet)
        {
            if (!ShowHelp) return;

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
            var optionSet = new OptionSet()
                                {
                                    {"t=|traceLevel=", "The level of verbosity on output. Valid values are Off, Info, Warning, Error, Verbose. Default is Info.\n", v =>
                                        {
                                            var traceLevel = ConvertStringToTraceLevel(v);
                                            Logger.TraceLevel = traceLevel;
                                            _options.TraceLevel = traceLevel;
                                        }},
                                    {"w|webDeployExist", "Tells ConDep not to deploy WebDeploy, because it already exist on server.\n", v => _options.WebDeployExist = v != null },
                                    {"q=|webQ=", "Will use ConDep's Web Queue to queue the deployment, preventing multiple deployments to execute at the same time. Useful when ConDep is triggered often from CI environments. Expects the url for the WebQ as its value.\n", v => _options.WebQAddress = v },
                                    {"d|deployOnly", "Deploy all except infrastructure\n", v => _options.DeployOnly = v != null},
                                    {"b|bypassLB", "Don't use configured load balancer during execution.\n", v => _options.BypassLB = v != null},
                                    {"s|sams|stopAfterMarkedServer", "Will only deploy to server marked as StopServer in json config, or first server if no server is marked. After execution, run ConDep with the continueAfterMarkedServer switch to continue deployment to remaining servers.\n", v => _options.StopAfterMarkedServer = v != null},
                                    {"c|cams|continueAfterMarkedServer", "Will continue deployment to remaining servers. Used after ConDep has previously executed with the stopAfterMarkedServer switch.\n", v => _options.ContinueAfterMarkedServer = v != null},
                                    {"h|?|help",  "show this message and exit", v => ShowHelp = v != null }
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

        private bool ShowHelp { get; set; }

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
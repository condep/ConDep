using System;
using System.Diagnostics;
using ConDep.Dsl;
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
        }

        private void ValidateEnvironment(OptionSet optionSet)
        {
            Params.Environment = _args[1];
            if (string.IsNullOrWhiteSpace(Params.Environment))
            {
                System.Console.Error.WriteLine("No environment provided.");
                System.Console.Error.WriteLine();
                _helpWriter.PrintHelp(optionSet);
                Environment.Exit(1);
            }
        }

        private void ValidateAssemblyName(OptionSet optionSet)
        {
            Params.AssemblyName = _args[0];
            if (string.IsNullOrWhiteSpace(Params.AssemblyName))
            {
                System.Console.Error.WriteLine("No assembly provided.");
                System.Console.Error.WriteLine();
                _helpWriter.PrintHelp(optionSet);
                Environment.Exit(1);
            }
        }

        private void CheckForAndShowHelp(OptionSet optionSet)
        {
            if (!Params.ShowHelp) return;

            _helpWriter.PrintHelp(optionSet);
            Environment.Exit(0);
        }

        private void ValidateNumberOfParams(OptionSet optionSet)
        {
            if (_args.Length >= 2) return;

            _helpWriter.PrintHelp(optionSet);
            Environment.Exit(1);
        }

        private OptionSet GetOptionSet()
        {
            Params = new CommandLineParams(); 

            var optionSet = new OptionSet()
                                {
                                    {"s=|server=", "Server to deploy to", v => Params.Server = v},
                                    {"c=|context=", "Context to deploy", v => Params.Context = v},
                                    {"t=|traceLevel=", "The level of verbosity on output. Valid values are Off, Info, Warning, Error, Verbose. Default is Info.", v => Logger.TraceLevel = ConvertStringToTraceLevel(v)},
                                    {"infraOnly", "Deploy infrastructure only", v => Params.InfraOnly = v != null },
                                    {"deployOnly", "Deploy all except infrastructure", v => Params.DeployOnly = v != null},
                                    {"bypassLB", "Don't use configured load balancer during execution.", v => Params.BypassLB = v != null},
                                    {"p|printSequence", "Prints the execution sequence of all operations and providers.", v => Params.PrintSequence = v != null},
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
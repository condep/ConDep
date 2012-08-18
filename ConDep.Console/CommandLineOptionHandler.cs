using System;
using System.Diagnostics;
using NDesk.Options;

namespace ConDep.Console
{
    internal class CommandLineOptionHandler
    {
        private readonly string[] _args;
        private CommandLineParams _params;
        private bool _initialized;

        public CommandLineOptionHandler(string[] args)
        {
            _args = args;
        }

        public CommandLineParams Params
        {
            get
            {
                if(!_initialized)
                {
                    Init();
                    _initialized = true;
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
                PrintHelp(optionSet);
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
                PrintHelp(optionSet);
                Environment.Exit(1);
            }
        }

        private void CheckForAndShowHelp(OptionSet optionSet)
        {
            if (!Params.ShowHelp) return;
            
            PrintHelp(optionSet);
            Environment.Exit(0);
        }

        private void ValidateNumberOfParams(OptionSet optionSet)
        {
            if (_args.Length >= 2) return;
            
            PrintHelp(optionSet);
            Environment.Exit(1);
        }

        private OptionSet GetOptionSet()
        {
            Params = new CommandLineParams(); 

            var optionSet = new OptionSet()
                                {
                                    {"s=|server=", "Server to deploy to", v => Params.Server = v},
                                    {"a=|application=", "Application to deploy", v => Params.Application = v},
                                    {"t=|traceLevel=", "The level of verbosity on output. Valid values are Off, Info, Warning, Error, Verbose. Default is Info.", v => Params.TraceLevel = ConvertStringToTraceLevel(v)},
                                    {"infraOnly", "Deploy infrastructure only", v => Params.InfraOnly = v != null },
                                    {"deployOnly", "Deploy all except infrastructure", v => Params.DeployOnly = v != null},
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

        private static void PrintHelp(OptionSet optionSet)
        {
            System.Console.WriteLine();
            System.Console.WriteLine("Deploy files and infrastructure to remote servers and environments");
            System.Console.WriteLine();
            System.Console.WriteLine("Usage: ConDep <assembly> <environment> [-options]");
            System.Console.WriteLine();
            System.Console.WriteLine("  <assembly>\t\tAssembly containing deployment setup");
            System.Console.WriteLine("  <environment>\t\tEnvironment to deploy to (e.g. Dev, Test etc)");
            System.Console.WriteLine();
            System.Console.WriteLine("where options include:");
            optionSet.WriteOptionDescriptions(System.Console.Out);
            System.Console.WriteLine();

            System.Console.WriteLine("Examples:");
            System.Console.WriteLine("\t(1) ConDep.exe MyAssembly.dll Dev");
            System.Console.WriteLine();
            System.Console.WriteLine("\t(2) ConDep.exe MyAssembly.dll Dev -s MyWebServer");
            System.Console.WriteLine();
            System.Console.WriteLine("\t(3) ConDep.exe MyAssembly.dll Dev -s MyWebServer -a MyWebApp");
            System.Console.WriteLine();
            System.Console.WriteLine("\t(4) ConDep.exe MyAssembly.dll Dev -a MyWebApp");
            System.Console.WriteLine();
            System.Console.WriteLine("\t(5) ConDep.exe MyAssembly.dll Dev -a MyWebApp -d");
            System.Console.WriteLine();
            System.Console.WriteLine("\t(6) ConDep.exe MyAssembly.dll Dev -a MyWebApp -i");
            System.Console.WriteLine();
            System.Console.WriteLine("\t(7) ConDep.exe MyAssembly.dll Dev -i");
            System.Console.WriteLine();
            System.Console.WriteLine("\t(8) ConDep.exe MyAssembly.dll Dev -d");
            System.Console.WriteLine();

            System.Console.WriteLine("Explanations:");
            System.Console.WriteLine("\t1 - Deploy setup found in MyAssembly.dll to all servers in");
            System.Console.WriteLine("\t    the Dev environment.");
            System.Console.WriteLine();
            System.Console.WriteLine("\t2 - Deploy setup found in MyAssembly.dll do the Dev environment, ");
            System.Console.WriteLine("\t    but only to the server MyWebServer.");
            System.Console.WriteLine();
            System.Console.WriteLine("\t3 - Same as above, except only deploys the application MyWebApp.");
            System.Console.WriteLine("\t    This also meens no infrastructure is deployed (-do option).");
            System.Console.WriteLine();
            System.Console.WriteLine("\t4 - Deploy the application MyWebApp to all servers in the");
            System.Console.WriteLine("\t    Dev environment. (here the -do option is implisit).");
            System.Console.WriteLine();
            System.Console.WriteLine("\t5 - Same as above, only here -do is explicitly set.");
            System.Console.WriteLine();
            System.Console.WriteLine("\t6 - This will result in an error, cause you cannot deploy");
            System.Console.WriteLine("\t    an application with the infrastructure only option set.");
            System.Console.WriteLine();
            System.Console.WriteLine("\t7 - Deploy infrastructure setup only.");
            System.Console.WriteLine();
            System.Console.WriteLine("\t8 - Will only deploy deployment specific setup and");
            System.Console.WriteLine("\t    not infrastrucutre.");
        }

    }
}
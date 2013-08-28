using System;
using System.Diagnostics;
using ConDep.Dsl.Config;
using ConDep.Dsl.Logging;
using NDesk.Options;

namespace ConDep.Console
{
    internal sealed class CommandLineParser
    {
        private readonly string[] _args;
        private readonly ConDepOptions _deployOptions = new ConDepOptions();
        private readonly EncryptOptions _encryptOptions = new EncryptOptions();
        private readonly DecryptOptions _decryptOptions = new DecryptOptions();
        private readonly HelpWriter _helpWriter;

        private CommandLineParser(string[] args)
        {
            _args = args;
            _helpWriter = new HelpWriter(System.Console.Out);
        }

        //private OptionSet GetOptionSet(string[] args)
        //{
        //    switch (GetCommand(args))
        //    {
        //        case ConDepCommand.Deploy:
        //            return GetDeployOptionSet();
        //        case ConDepCommand.Encrypt:
        //            return GetEncryptOptionSet();
        //        case ConDepCommand.Decrypt:
        //            return GetDecryptOptionSet();
        //        case ConDepCommand.Help:
        //            return new OptionSet();
        //        //    //_helpWriter.PrintHelp(_args[1]);
        //        //default:
        //        //    ExitWithMessage("No command specified. You need to provide either Deploy, Encrypt or Decrypt as the first parameter.");
        //        //    return null;
        //    }
        //    throw new ConDepHelpException("No command specified. You need to provide either Deploy, Encrypt or Decrypt as the first parameter.");
        //}

        private void ValidateParams(OptionSet optionSet)
        {
            if(WebQInstallParamExist()) return;

            //ValidateNumberOfParams(optionSet);
            CheckForAndShowHelp(optionSet);
            //ValidateAssemblyName(optionSet);
            //ValidateEnvironment(optionSet);
            //ValidateApplication(optionSet);
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
                DeployOptions.InstallWebQ = true;
            }
            else
            {
                return false;
            }

            if(_args.Length == 2)
            {
                DeployOptions.InstallWebQOnServer = _args[1];
            }
            return DeployOptions.InstallWebQ;
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

        private void ExitWithMessage(string message)
        {
            System.Console.Error.WriteLine(message);
            System.Console.Error.WriteLine();
            _helpWriter.PrintRootHelp();
            Environment.Exit(1);
        }

        private OptionSet GetEncryptOptionSet()
        {
            var optionSet = new OptionSet()
                                {
                                    {"d=|dir=", "Directory where env.json files are located\n", v => EncryptOptions.Directory = v},
                                    {"k=|key=", "Key to use for encryption - if not provided one will be generated for you\n", v=> DecryptOptions.Key = v}
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

        private OptionSet GetDecryptOptionSet()
        {
            var optionSet = new OptionSet()
                                {
                                    {"d=|dir=", "Directory where env.json files are located\n", v => EncryptOptions.Directory = v},
                                    {"k=|key=", "Key to use for decryption\n", v=> DecryptOptions.Key = v}
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
        
        private OptionSet GetDeployOptionSet()
        {
            var optionSet = new OptionSet()
                                {
                                    {"t=|traceLevel=", "The level of verbosity on output. Valid values are Off, Info, Warning, Error, Verbose. Default is Info.\n", v =>
                                        {
                                            var traceLevel = ConvertStringToTraceLevel(v);
                                            Logger.TraceLevel = traceLevel;
                                            DeployOptions.TraceLevel = traceLevel;
                                        }},
                                    {"k=|cryptoKey=", "Key used to decrypt passwords and other sensitive date in ConDep json-config files.", v=> DeployOptions.CryptoKey = v},
                                    {"q=|webQ=", "Will use ConDep's Web Queue to queue the deployment, preventing multiple deployments to execute at the same time. Useful when ConDep is triggered often from CI environments. Expects the url for the WebQ as its value.\n", v => DeployOptions.WebQAddress = v },
                                    {"d|deployOnly", "Deploy all except infrastructure\n", v => DeployOptions.DeployOnly = v != null},
                                    {"b|bypassLB", "Don't use configured load balancer during execution.\n", v => DeployOptions.BypassLB = v != null},
                                    {"s|sams|stopAfterMarkedServer", "Will only deploy to server marked as StopServer in json config, or first server if no server is marked. After execution, run ConDep with the continueAfterMarkedServer switch to continue deployment to remaining servers.\n", v => DeployOptions.StopAfterMarkedServer = v != null},
                                    {"c|cams|continueAfterMarkedServer", "Will continue deployment to remaining servers. Used after ConDep has previously executed with the stopAfterMarkedServer switch.\n", v => DeployOptions.ContinueAfterMarkedServer = v != null},
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

        public ConDepOptions DeployOptions
        {
            get { return _deployOptions; }
        }

        public EncryptOptions EncryptOptions
        {
            get { return _encryptOptions; }
        }

        public DecryptOptions DecryptOptions
        {
            get { return _decryptOptions; }
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

    internal class ConDepHelpException : Exception
    {
        public ConDepHelpException(string message) : base(message)
        {
            
        }
    }

    internal class DecryptOptions
    {
        public string Key { get; set; }
    }

    internal class EncryptOptions
    {
        public string Directory { get; set; }
    }
}
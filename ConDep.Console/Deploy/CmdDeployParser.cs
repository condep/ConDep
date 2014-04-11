using System;
using System.Diagnostics;
using System.IO;
using ConDep.Dsl.Config;
using ConDep.Dsl.Logging;
using NDesk.Options;

namespace ConDep.Console
{
    public class CmdDeployParser : CmdBaseParser<ConDepOptions>
    {
        const int MIN_ARGS_REQUIRED = 3;
        private const string DEPLOY_ALL_APPS_SWITCH = "--deployAllApps";

        private readonly ConDepOptions _options = new ConDepOptions();
        private readonly OptionSet _optionSet;

        public CmdDeployParser(string[] args) : base(args)
        {
            _optionSet = new OptionSet()
                {
                    {"t=|traceLevel=", "The level of verbosity on output. Valid values are Off, Info, Warning, Error, Verbose. Default is Info.\n", v =>
                        {
                            var traceLevel = ConvertStringToTraceLevel(v);
                            Logger.TraceLevel = traceLevel;
                            _options.TraceLevel = traceLevel;
                        }},
                    {"k=|cryptoKey=", "Key used to decrypt passwords and other sensitive date in ConDep json-config files.", v=> _options.CryptoKey = v},
                    {"q=|webQ=", "Will use ConDep's Web Queue to queue the deployment, preventing multiple deployments to execute at the same time. Useful when ConDep is triggered often from CI environments. Expects the url for the WebQ as its value.\n", v => _options.WebQAddress = v },
                    {"d|deployOnly", "Deploy all except infrastructure\n", v => _options.DeployOnly = v != null},
                    {"b|bypassLB", "Don't use configured load balancer during execution.\n", v => _options.BypassLB = v != null},
                    {"s|sams|stopAfterMarkedServer", "Will only deploy to server marked as StopServer in json config, or first server if no server is marked. After execution, run ConDep with the continueAfterMarkedServer switch to continue deployment to remaining servers.\n", v => _options.StopAfterMarkedServer = v != null},
                    {"c|cams|continueAfterMarkedServer", "Will continue deployment to remaining servers. Used after ConDep has previously executed with the stopAfterMarkedServer switch.\n", v => _options.ContinueAfterMarkedServer = v != null},
                    {"dryrun", "Will output the execution sequence without actually executing it.", v => _options.DryRun = v != null},
                    {"apiTimeout=", "Timeout in seconds before calls to the ConDep server api (ConDep Node) will time out (default is 100 seconds).", v => _options.ApiTimout = Convert.ToInt32(v) }
                };

        }

        public override OptionSet OptionSet
        {
            get { return _optionSet; }
        }

        public override ConDepOptions Parse()
        {
            if (_args.Length < MIN_ARGS_REQUIRED)
                throw new ConDepCmdParseException(string.Format("The Deploy command requires at least {0} arguments.", MIN_ARGS_REQUIRED));

            _options.AssemblyName = _args[0];
            _options.Environment = _args[1];
            var appParam = _args[2];

            if (!string.IsNullOrWhiteSpace(appParam) && appParam.ToLower() == DEPLOY_ALL_APPS_SWITCH.ToLower())
            {
                _options.DeployAllApps = true;
            }
            else
            {
                _options.Application = appParam;
            }

            try
            {
                OptionSet.Parse(_args);
            }
            catch (OptionException oe)
            {
                throw new ConDepCmdParseException("Unable to successfully parse arguments.", oe);
            }
            return _options;
        }

        private static TraceLevel ConvertStringToTraceLevel(string traceLevel)
        {
            if (string.IsNullOrWhiteSpace(traceLevel)) return TraceLevel.Info;

            switch (traceLevel.ToLower())
            {
                case "off": return TraceLevel.Off;
                case "warning": return TraceLevel.Warning;
                case "error": return TraceLevel.Error;
                case "verbose": return TraceLevel.Verbose;
                default: return TraceLevel.Info;
            }
        }

        public override void WriteOptionsHelp(TextWriter writer)
        {
            OptionSet.WriteOptionDescriptions(writer);
        }
    }
}
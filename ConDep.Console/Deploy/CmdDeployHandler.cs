using System;
using System.Threading;
using System.Threading.Tasks;
using ConDep.Dsl.Config;
using ConDep.Dsl.Logging;
using ConDep.Dsl.Remote;
using ConDep.Dsl.SemanticModel;
using ConDep.Dsl.SemanticModel.WebDeploy;
using ConDep.WebQ.Client;

namespace ConDep.Console.Deploy
{
    public class CmdDeployHandler : IHandleConDepCommands
    {
        private CmdDeployParser _parser;
        private CmdDeployValidator _validator;
        private CmdDeployHelpWriter _helpWriter;
        private WebQueue _webQ;
        private CancellationTokenSource _tokenSource;

        public CmdDeployHandler(string[] args)
        {
            _parser = new CmdDeployParser(args);
            _validator = new CmdDeployValidator();
            _helpWriter = new CmdDeployHelpWriter(System.Console.Out);
        }

        public void Execute(CmdHelpWriter helpWriter, ILogForConDep logger)
        {
            var conDepSettings = new ConDepSettings();
            bool success;

            try
            {
                conDepSettings.Options = GetOptions(_parser, _validator);
                conDepSettings.Config = ConfigHandler.GetEnvConfig(conDepSettings);

                helpWriter.PrintCopyrightMessage();
                _webQ = WaitInQueue(logger, conDepSettings);

                var status = new ConDepStatus();
                _tokenSource = new CancellationTokenSource();
                var token = _tokenSource.Token;

                var task = ConDepConfigurationExecutor.ExecuteFromAssembly(conDepSettings, status, token);

                task.ContinueWith(result =>
                        {
                            if (result.Result.Success)
                            {
                                status.EndTime = DateTime.Now;
                                status.PrintSummary();
                            }
                            else
                            {
                                Environment.Exit(1);
                            }
                        }, TaskContinuationOptions.OnlyOnRanToCompletion);

                task.Wait();

                //while (!task.IsCanceled && !task.IsCompleted && !task.IsFaulted)
                //{
                //    Thread.Sleep(1000);
                //}

                if (task.IsFaulted)
                {
                    throw task.Exception;
                }
            }
            finally
            {
                if (_webQ != null)
                {
                    _webQ.LeaveQueue();
                }

                //Environment.Exit(exitCode);
            }

        }

        public void Cancel()
        {
            if (_tokenSource != null)
            {
                _tokenSource.Cancel();
            }

            if (_webQ != null)
            {
                Logger.Info("Leaving queue...");
                _webQ.LeaveQueue();
            }
        }
        public void WriteHelp()
        {
            _helpWriter.WriteHelp(_parser.OptionSet);
        }

        private ConDepOptions GetOptions(CmdBaseParser<ConDepOptions> parser, CmdBaseValidator<ConDepOptions> validator)
        {
            var options = parser.Parse();
            validator.Validate(options);

            var configAssemblyLoader = new ConDepAssemblyHandler(options.AssemblyName);
            options.Assembly = configAssemblyLoader.GetAssembly();

            return options;
        }

        private static WebQueue WaitInQueue(ILogForConDep logger, ConDepSettings conDepSettings)
        {
            if (!string.IsNullOrWhiteSpace(conDepSettings.Options.WebQAddress))
            {
                var webQ = new WebQueue(conDepSettings.Options.WebQAddress, conDepSettings.Options.Environment);
                webQ.WebQueuePositionUpdate += (sender, eventArgs) => logger.Info(eventArgs.Message);
                webQ.WebQueueTimeoutUpdate += (sender, eventArgs) => logger.Info(eventArgs.Message);
                logger.LogSectionStart("Waiting in Deployment Queue");
                try
                {
                    webQ.WaitInQueue(TimeSpan.FromMinutes(30));
                }
                finally
                {
                    logger.LogSectionEnd("Waiting in Deployment Queue");
                }
                return webQ;
            }
            return null;
        }
    }
}
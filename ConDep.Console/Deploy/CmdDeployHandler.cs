using System;
using System.IO;
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

        public CmdDeployHandler(string[] args)
        {
            _parser = new CmdDeployParser(args);
            _validator = new CmdDeployValidator();
            _helpWriter = new CmdDeployHelpWriter(System.Console.Out);
        }

        public void Execute(CmdHelpWriter helpWriter, ILogForConDep logger)
        {
            WebQueue webQ = null;
            var conDepSettings = new ConDepSettings();

            try
            {
                conDepSettings.Options = GetOptions(_parser, _validator);
                conDepSettings.Config = ConfigHandler.GetEnvConfig(conDepSettings);

                helpWriter.PrintCopyrightMessage();
                webQ = WaitInQueue(logger, conDepSettings);

                var status = new ConDepStatus();
                var clientValidator = new ClientValidator();

                var serverInfoHarvester = new ServerInfoHarvester(conDepSettings);
                var serverValidator = new RemoteServerValidator(conDepSettings.Config.Servers, serverInfoHarvester);

                ConDepConfigurationExecutor.ExecuteFromAssembly(conDepSettings, status, clientValidator, serverValidator);

                //if (status.HasErrors)
                //{
                //    throw status.
                //}
                //else
                //{
                status.EndTime = DateTime.Now;
                status.PrintSummary();
                //}
            }
            finally
            {
                if (webQ != null)
                {
                    webQ.LeaveQueue();
                }

                //Environment.Exit(exitCode);
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
using System.IO;
using ConDep.Dsl.Logging;

namespace ConDep.Console.Help
{
    public class CmdHelpHandler : IHandleConDepCommands
    {
        private readonly CmdHelpParser _parser;
        private readonly CmdHelpValidator _validator;
        private CmdHelpWriter _helpWriter;

        public CmdHelpHandler(string[] args)
        {
            _parser = new CmdHelpParser(args);
            _validator = new CmdHelpValidator();
        }

        public void Execute(CmdHelpWriter helpWriter, ILogForConDep logger)
        {
            _helpWriter = helpWriter;
            var options = _parser.Parse();
            _validator.Validate(options);

            if (options.NoOptions())
            {
                helpWriter.WriteHelp();
            }
            else
            {
                helpWriter.WriteHelpForCommand(options.Command);
            }

        }

        public void WriteHelp()
        {
            _helpWriter.WriteHelp();
        }
    }
}
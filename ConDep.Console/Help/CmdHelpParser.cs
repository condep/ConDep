using System;
using System.IO;
using NDesk.Options;

namespace ConDep.Console.Help
{
    public class CmdHelpParser : CmdBaseParser<ConDepHelpOptions>
    {
        private OptionSet _optionSet;

        public CmdHelpParser(string[] args) : base(args)
        {
            _optionSet = new OptionSet();
        }

        public override OptionSet OptionSet
        {
            get { return _optionSet; }
        }

        public override ConDepHelpOptions Parse()
        {
            var options = new ConDepHelpOptions();
            if (_args == null || _args.Length == 0)
            {
                return options;
            }

            var command = _args[0].Trim().ToLower();
            if (command == "deploy")
            {
                options.Command = ConDepCommand.Deploy;
            }
            else if (command == "encrypt")
            {
                options.Command = ConDepCommand.Encrypt;
            }
            else if (command == "decrypt")
            {
                options.Command = ConDepCommand.Decrypt;
            }
            else
            {
                throw new ConDepCmdParseException(string.Format("The command [{0}] is unknown to ConDep and unable to show help for command.", command));
            }
            return options;
        }

        public override void WriteOptionsHelp(TextWriter writer)
        {
            throw new System.NotImplementedException();
        }
    }
}
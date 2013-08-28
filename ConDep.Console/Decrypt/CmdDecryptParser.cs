using System.IO;
using NDesk.Options;

namespace ConDep.Console.Decrypt
{
    public class CmdDecryptParser : CmdBaseParser<ConDepDecryptOptions>
    {
        private ConDepDecryptOptions _options;
        private OptionSet _optionSet;
        const int MIN_ARGS_REQUIRED = 1;

        public CmdDecryptParser(string[] args) : base(args)
        {
            _options = new ConDepDecryptOptions();

            _optionSet = new OptionSet()
                {
                    {"e=|env=", "Environment file to decrypt. Default is all. Valid options are All or spesific environment like dev.", v => _options.Env = v},
                    {"d=|dir=", "Directory where environment config files are located. If no directory is provided, current directory will be used.", v => _options.Dir = v}
                };
        }

        public override OptionSet OptionSet
        {
            get { return _optionSet; }
        }

        public override ConDepDecryptOptions Parse()
        {
            try
            {
                if (_args.Length < MIN_ARGS_REQUIRED)
                    throw new ConDepCmdParseException(string.Format("The Encrypt command requires at least a key as argument.", MIN_ARGS_REQUIRED));

                _options.Key = _args[0];
                _optionSet.Parse(_args);
            }
            catch (OptionException oe)
            {
                throw new ConDepCmdParseException("Unable to successfully parse arguments.", oe);
            }
            return _options;
        }

        public override void WriteOptionsHelp(TextWriter writer)
        {
        }
    }
}
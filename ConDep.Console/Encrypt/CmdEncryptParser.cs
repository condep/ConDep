using System.IO;
using NDesk.Options;

namespace ConDep.Console.Encrypt
{
    public class CmdEncryptParser : CmdBaseParser<ConDepEncryptOptions>
    {
        private ConDepEncryptOptions _options;
        private OptionSet _optionSet;

        public CmdEncryptParser(string[] args) : base(args)
        {
            _options = new ConDepEncryptOptions();

            _optionSet = new OptionSet()
                {
                    {"e=|env=", "Environment file to encrypt. Default is all. Valid options are All or spesific environment like dev.", v => _options.Env = v},
                    {"d=|dir=", "Directory where environment config files are located. If no directory is provided, current directory will be used.", v => _options.Dir = v},
                    {"k=|key=", "Optional key to use for encryption. If not provided, a key will be generated and returned for your convenience. Keep key secure since it's needed for deployment and decryption.", v => _options.Key = v},
                    {"q|quiet", "Skips confirmation before encrypting.", v => _options.Quiet = v != null}
                };
        }

        public override OptionSet OptionSet
        {
            get { return _optionSet; }
        }

        public override ConDepEncryptOptions Parse()
        {
            try
            {
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
            _optionSet.WriteOptionDescriptions(writer);
        }
    }
}
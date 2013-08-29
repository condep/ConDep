using System.Collections.Generic;
using System.IO;
using System.Linq;
using ConDep.Dsl.Config;
using ConDep.Dsl.Logging;
using ConDep.Dsl.Security;
using Newtonsoft.Json.Linq;

namespace ConDep.Console.Decrypt
{
    public class CmdDecryptHandler : IHandleConDepCommands
    {
        private CmdDecryptParser _parser;
        private CmdDecryptValidator _validator;
        private CmdDecryptHelpWriter _helpWriter;

        public CmdDecryptHandler(string[] args)
        {
            _parser = new CmdDecryptParser(args);
            _validator = new CmdDecryptValidator();
            _helpWriter = new CmdDecryptHelpWriter(System.Console.Out);
        }

        public void Execute(CmdHelpWriter helpWriter, ILogForConDep logger)
        {
            var options = _parser.Parse();
            _validator.Validate(options);

            var crypto = new JsonPasswordCrypto(options.Key);

            var configParser = new EnvConfigParser();
            foreach (var file in configParser.GetConDepConfigFiles(options.Dir))
            {
                System.Console.Out.WriteLine("\tDecrypting file [{0}] ...", file);
                configParser.DecryptFile(file, crypto);
                System.Console.Out.WriteLine("\tFile decrypted.");
                System.Console.WriteLine();
            }
        }

        public void WriteHelp()
        {
            _helpWriter.WriteHelp(_parser.OptionSet);
        }
    }
}
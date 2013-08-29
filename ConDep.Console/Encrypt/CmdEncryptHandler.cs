using System;
using System.Linq;
using ConDep.Dsl.Config;
using ConDep.Dsl.Logging;
using ConDep.Dsl.Security;

namespace ConDep.Console.Encrypt
{
    public class CmdEncryptHandler : IHandleConDepCommands
    {
        private CmdEncryptParser _parser;
        private CmdEncryptValidator _validator;
        private CmdEncryptHelpWriter _helpWriter;

        public CmdEncryptHandler(string[] args)
        {
            _parser = new CmdEncryptParser(args);
            _validator = new CmdEncryptValidator();
            _helpWriter = new CmdEncryptHelpWriter(System.Console.Out);
        }

        public void Execute(CmdHelpWriter helpWriter, ILogForConDep logger)
        {
            var options = _parser.Parse();
            _validator.Validate(options);

            var key = GetKey(options);
            var crypto = new JsonPasswordCrypto(key);

            var configParser = new EnvConfigParser();
            bool anySuccess = false;

            var files = configParser.GetConDepConfigFiles(options.Dir).ToList();

            helpWriter.PrintCopyrightMessage();
            System.Console.WriteLine();

            if (!options.Quiet)
            {
                System.Console.WriteLine("The following files will be encrypted:");
                files.ForEach(x => System.Console.WriteLine("\t{0}", x));

                System.Console.Write("\nContinue? (y/n) : ");
                var choice = System.Console.Read();

                if (Convert.ToChar(choice) != 'y')
                {
                    return;
                }

                System.Console.WriteLine();
            }

            foreach (var file in files)
            {
                System.Console.Out.WriteLine("\tEncrypting file [{0}] ...", file);
                try
                {
                    configParser.EncryptFile(file, crypto);
                    anySuccess = true;
                    System.Console.Out.WriteLine("\tFile encrypted.");
                }
                catch (ConDepCryptoException ex)
                {
                    System.Console.ForegroundColor = ConsoleColor.Red;
                    System.Console.Error.WriteLine("\tError: " + ex.Message);
                }
                System.Console.Out.WriteLine();
                System.Console.ResetColor();
            }

            if (anySuccess)
            {
                System.Console.ForegroundColor = ConsoleColor.Green;
                System.Console.Out.WriteLine("\tEncryption key: {0}", key);
                System.Console.Out.WriteLine("\tKeep this key safe!");
                System.Console.Out.WriteLine();
                System.Console.Out.WriteLine("\tWhen deploying or decrypting, use the /key option to provide key.");
                System.Console.ResetColor();
            }
        }

        public void WriteHelp()
        {
            _helpWriter.WriteHelp(_parser.OptionSet);
        }

        private string GetKey(ConDepEncryptOptions options)
        {
            return string.IsNullOrWhiteSpace(options.Key) ? JsonPasswordCrypto.GenerateKey(256) : options.Key;
        }
    }
}
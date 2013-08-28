using System;
using System.IO;
using ConDep.Dsl.Config;
using ConDep.Dsl.Logging;
using ConDep.Dsl.Security;
using Newtonsoft.Json.Linq;

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
            System.Console.Out.WriteLine();
            helpWriter.PrintCopyrightMessage();
            System.Console.Out.WriteLine();

            var options = _parser.Parse();
            _validator.Validate(options);

            var key = GetKey(options);
            var crypto = new JsonPasswordCrypto(key);

            var configParser = new EnvConfigParser();
            bool anySuccess = false;

            foreach (var file in configParser.GetConDepConfigFiles(options.Dir))
            {
                System.Console.Out.WriteLine("\tEncrypting file [{0}] ...", file);
                try
                {
                    EncryptFile(configParser, file, crypto);
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

        private void EncryptFile(EnvConfigParser parser, string file, JsonPasswordCrypto crypto)
        {
            var json = parser.GetConDepConfigAsJsonText(file);
            dynamic config = JObject.Parse(json);

            if (parser.AlreadyEncrypted(config))
                throw new ConDepCryptoException(string.Format("File [{0}] already encrypted.", file));

            foreach (var server in config.Servers)
            {
                if (server.DeploymentUser != null)
                {
                    EncryptJsonValue(crypto, server.DeploymentUser.Password);
                }
            }

            EncryptJsonValue(crypto, config.DeploymentUser.Password);
            parser.UpdateConfig(file, config);
        }

        private static void EncryptJsonValue(JsonPasswordCrypto crypto, JValue valueToEncrypt)
        {
            var value = valueToEncrypt.Value<string>();
            var encryptedValue = crypto.Encrypt(value);
            valueToEncrypt.Replace(JObject.FromObject(encryptedValue));
        }

        private string GetKey(ConDepEncryptOptions options)
        {
            return string.IsNullOrWhiteSpace(options.Key) ? JsonPasswordCrypto.GenerateKey(32) : options.Key;
        }
    }
}
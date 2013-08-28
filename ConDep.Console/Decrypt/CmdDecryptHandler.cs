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

            var key = options.Key;
            var crypto = new JsonPasswordCrypto(key);

            var configParser = new EnvConfigParser();
            foreach (var file in configParser.GetConDepConfigFiles(options.Dir))
            {
                DecryptFile(configParser, file, crypto);
            }
        }

        public void WriteHelp()
        {
            _helpWriter.WriteHelp(_parser.OptionSet);
        }

        private void DecryptFile(EnvConfigParser parser, string file, JsonPasswordCrypto crypto)
        {
            var json = parser.GetConDepConfigAsJsonText(file);
            dynamic config = JObject.Parse(json);

            if (!parser.AlreadyEncrypted(config))
                return;

            foreach (var server in config.Servers)
            {
                if (server.DeploymentUser != null)
                {
                    DecryptJsonValue(crypto, server.DeploymentUser.Password);
                }
            }

            DecryptJsonValue(crypto, config.DeploymentUser.Password);
            parser.UpdateConfig(file, config);
        }

        private void DecryptJsonValue(JsonPasswordCrypto crypto, dynamic originalValue)
        {
            var valueToDecrypt = new EncryptedPassword(originalValue.IV.Value, originalValue.Password.Value);
            var decryptedValue = crypto.Decrypt(valueToDecrypt);
            JObject valueToReplace = originalValue;
            valueToReplace.Replace(decryptedValue);
        }
    }
}
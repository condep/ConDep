using System.Collections.Generic;
using System.IO;
using System.Linq;
using ConDep.Dsl.Security;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ConDep.Dsl.Config
{
    public class EnvConfigParser
    {
        private JsonSerializerSettings _jsonSettings;

        public void UpdateConfig(string filePath, dynamic config)
        {
            var jsonText = JsonConvert.SerializeObject(config, JsonSettings);
            File.WriteAllText(filePath, jsonText);
        }

        public bool Encrypted(string jsonConfig, out dynamic jsonModel)
        {
            jsonModel = JObject.Parse(jsonConfig);

            if (jsonModel.DeploymentUser != null)
            {
                if (!(jsonModel.DeploymentUser.Password.Value is string))
                {
                    return true;
                }
            }
            if (jsonModel.Servers != null)
            {
                foreach (var server in jsonModel.Servers)
                {
                    if (server.DeploymentUser != null)
                    {
                        if (!(server.DeploymentUser.Password.Value is string))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public IEnumerable<string> GetConDepConfigFiles(string directory = null)
        {
            var dir = string.IsNullOrWhiteSpace(directory) ? Directory.GetCurrentDirectory() : directory;
            if (!Directory.Exists(dir))
                throw new DirectoryNotFoundException(string.Format("Tried to find ConDep config files in directory [{0}], but directory does not exist.", dir));

            var dirInfo = new DirectoryInfo(dir);
            var configFiles = dirInfo.GetFiles("*.env.json");

            if (!configFiles.Any())
                throw new FileNotFoundException(string.Format("No ConDep configuration files found in directory [{0}]", dir));

            return configFiles.Select(x => x.FullName);
        }


        public ConDepEnvConfig GetTypedEnvConfig(string filePath, string cryptoKey)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException(string.Format("[{0}] not found.", filePath), filePath);
            }

            using (var fileStream = File.OpenRead(filePath))
            {
                var crypto = new JsonPasswordCrypto(cryptoKey);
                return GetTypedEnvConfig(fileStream, crypto);
            }
        }

        public string GetConDepConfigAsJsonText(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException(string.Format("[{0}] not found.", filePath), filePath);
            }

            using (var fileStream = File.OpenRead(filePath))
            {
                using (var memStream = GetMemoryStreamWithCorrectEncoding(fileStream))
                {
                    using (var reader = new StreamReader(memStream))
                    {
                        return reader.ReadToEnd();
                    }
                }
            }
        }

        private JsonSerializerSettings JsonSettings
        {
            get
            {
                if (_jsonSettings == null)
                {
                    _jsonSettings = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore,
                        Formatting = Formatting.Indented,
                    };
                }
                return _jsonSettings;
            }
        }

        public void DecryptFile(string file, JsonPasswordCrypto crypto)
        {
            var json = GetConDepConfigAsJsonText(file);
            dynamic config;

            if (!Encrypted(json, out config))
            {
                throw new ConDepCryptoException("Unable to decrypt. No content in file [{0}] is encrypted.");                
            }

            DecryptJsonConfig(config, crypto);
            UpdateConfig(file, config);
        }

        public void DecryptJsonConfig(dynamic config, JsonPasswordCrypto crypto)
        {
            foreach (var server in config.Servers)
            {
                if (server.DeploymentUser != null)
                {
                    DecryptJsonValue(crypto, server.DeploymentUser.Password);
                }
            }

            DecryptJsonValue(crypto, config.DeploymentUser.Password);
        }

        private void DecryptJsonValue(JsonPasswordCrypto crypto, dynamic originalValue)
        {
            var valueToDecrypt = new EncryptedPassword(originalValue.IV.Value, originalValue.Password.Value);
            var decryptedValue = crypto.Decrypt(valueToDecrypt);
            JObject valueToReplace = originalValue;
            valueToReplace.Replace(decryptedValue);
        }

        public void EncryptFile(string file, JsonPasswordCrypto crypto)
        {
            var json = GetConDepConfigAsJsonText(file);
            dynamic config;

            if (Encrypted(json, out config))
                throw new ConDepCryptoException(string.Format("File [{0}] already encrypted.", file));

            EncryptJsonConfig(config, crypto);
            UpdateConfig(file, config);
        }

        public void EncryptJsonConfig(dynamic config, JsonPasswordCrypto crypto)
        {
            foreach (var server in config.Servers)
            {
                if (server.DeploymentUser != null)
                {
                    EncryptJsonValue(crypto, server.DeploymentUser.Password);
                }
            }

            EncryptJsonValue(crypto, config.DeploymentUser.Password);
        }

        private static void EncryptJsonValue(JsonPasswordCrypto crypto, JValue valueToEncrypt)
        {
            var value = valueToEncrypt.Value<string>();
            var encryptedValue = crypto.Encrypt(value);
            valueToEncrypt.Replace(JObject.FromObject(encryptedValue));
        }


        public ConDepEnvConfig GetTypedEnvConfig(Stream stream, JsonPasswordCrypto crypto)
        {

            ConDepEnvConfig config;
            using (var memStream = GetMemoryStreamWithCorrectEncoding(stream))
            {
                using (var reader = new StreamReader(memStream))
                {
                    var json = reader.ReadToEnd();
                    dynamic jsonModel;
                    if (Encrypted(json, out jsonModel))
                    {
                        DecryptJsonConfig(jsonModel, crypto);
                    }
                    config = JsonConvert.DeserializeObject<ConDepEnvConfig>(json, JsonSettings);
                }
            }

            if (config.Tiers == null)
            {
                foreach (var server in config.Servers)
                {
                    if (!server.DeploymentUser.IsDefined)
                    {
                        server.DeploymentUser = config.DeploymentUser;
                    }
                }
            }
            else
            {
                foreach (var tier in config.Tiers)
                {
                    foreach (var server in tier.Servers)
                    {
                        if (!server.DeploymentUser.IsDefined)
                        {
                            server.DeploymentUser = config.DeploymentUser;
                        }
                    }
                }
            }
            return config;

        }

        private static MemoryStream GetMemoryStreamWithCorrectEncoding(Stream stream)
        {
            using (var r = new StreamReader(stream, true))
            {
                var encoding = r.CurrentEncoding;
                return new MemoryStream(encoding.GetBytes(r.ReadToEnd()));
            }
        }
    }
}
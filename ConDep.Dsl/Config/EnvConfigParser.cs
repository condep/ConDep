using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace ConDep.Dsl.Config
{
    public class EnvConfigParser
    {
        public void UpdateConfig(string filePath, dynamic config)
        {
            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                Formatting = Formatting.Indented,
                //ContractResolver = new ConDepConfigContractResolver()
            };

            var jsonText = JsonConvert.SerializeObject(config, settings);
            File.WriteAllText(filePath, jsonText);
        }

        public bool AlreadyEncrypted(dynamic config)
        {
            if (config.DeploymentUser != null)
            {
                if (!(config.DeploymentUser.Password.Value is string))
                {
                    return true;
                }
            }
            if (config.Servers != null)
            {
                foreach (var server in config.Servers)
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


        public ConDepEnvConfig GetTypedEnvConfig(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException(string.Format("[{0}] not found.", filePath), filePath);
            }

            using (var fileStream = File.OpenRead(filePath))
            {
                return GetTypedEnvConfig(fileStream);
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
        public ConDepEnvConfig GetTypedEnvConfig(Stream stream)
        {

            ConDepEnvConfig config;
            using (var memStream = GetMemoryStreamWithCorrectEncoding(stream))
            {
                using (var reader = new StreamReader(memStream))
                {
                    var json = reader.ReadToEnd();
                    var settings = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore,
                        Formatting = Formatting.Indented,
                        //ContractResolver = new ConDepConfigContractResolver()
                    };

                    config = JsonConvert.DeserializeObject<ConDepEnvConfig>(json, settings);
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
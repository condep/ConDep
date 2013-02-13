using System.IO;
using System.Runtime.Serialization.Json;

namespace ConDep.Dsl.Config
{
    public class EnvConfigParser
    {
        public ConDepConfig GetEnvConfig(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException(string.Format("[{0}] not found.", filePath), filePath);
            }

            using (var fileStream = File.OpenRead(filePath))
            {
                return GetEnvConfig(fileStream);
            }
        }

        public ConDepConfig GetEnvConfig(Stream stream)
        {
            ConDepConfig config;
            using(var memStream = GetMemoryStreamWithCorrectEncoding(stream))
            {
                var serializer = new DataContractJsonSerializer(typeof(ConDepConfig));
                config = (ConDepConfig)serializer.ReadObject(memStream);
            }

            if(config.Tiers == null)
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
                foreach(var tier in config.Tiers)
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
using System.IO;
using System.Reflection;

namespace ConDep.Dsl.Config
{
    public class ConfigHandler
    {
        public static ConDepConfig GetEnvConfig(string environment, bool bypassLb, Assembly assembly)
        {
            var envFileName = string.Format("{0}.Env.json", environment);
            var envFilePath = Path.Combine(Path.GetDirectoryName(assembly.Location), envFileName);

            var jsonConfigParser = new EnvConfigParser();
            var envConfig = jsonConfigParser.GetEnvConfig(envFilePath);
            envConfig.EnvironmentName = environment;

            if (bypassLb)
            {
                envConfig.LoadBalancer = null;
            }
            return envConfig;
        }
    }
}
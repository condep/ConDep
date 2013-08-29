using System.IO;

namespace ConDep.Dsl.Config
{
    public static class ConfigHandler
    {
        public static ConDepEnvConfig GetEnvConfig(ConDepSettings settings)
        {
            var envFileName = string.Format("{0}.Env.json", settings.Options.Environment);
            var envFilePath = Path.Combine(Path.GetDirectoryName(settings.Options.Assembly.Location), envFileName);

            var jsonConfigParser = new EnvConfigParser();
            var envConfig = jsonConfigParser.GetTypedEnvConfig(envFilePath, settings.Options.CryptoKey);
            envConfig.EnvironmentName = settings.Options.Environment;

            if (settings.Options.BypassLB)
            {
                envConfig.LoadBalancer = null;
            }
            return envConfig;
        }
    }
}
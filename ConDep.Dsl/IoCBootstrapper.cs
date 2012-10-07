using System;
using ConDep.Dsl.LoadBalancer;
using ConDep.Dsl.Model.Config;
using ConDep.Dsl.WebDeploy;
using TinyIoC;

namespace ConDep.Dsl
{
    internal class IoCBootstrapper
    {
        private readonly ConDepConfig _envConfig;

        private IoCBootstrapper(ConDepConfig envConfig)
        {
            _envConfig = envConfig;
        }

        public void BootstrapTinyIoC()
        {
            var container = TinyIoCContainer.Current;

            container.Register(_envConfig);
            container.Register<ISetupWebDeploy, WebDeploySetup>().AsMultiInstance();
            container.Register<ISetupConDep, ConDepSetup>().AsMultiInstance();
            container.Register(_envConfig.LoadBalancer);
        }

        public static void Bootstrap(ConDepConfig envSettings)
        {
            new IoCBootstrapper(envSettings).BootstrapTinyIoC();
        }
    }
}
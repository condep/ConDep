using System;
using ConDep.Dsl.LoadBalancer;
using ConDep.Dsl.WebDeploy;
using TinyIoC;

namespace ConDep.Dsl
{
    public class IoCBootstrapper
    {
        private readonly ConDepEnvironmentSettings _envSettings;

        private IoCBootstrapper(ConDepEnvironmentSettings envSettings)
        {
            _envSettings = envSettings;
        }

        public void BootstrapTinyIoC()
        {
            var container = TinyIoCContainer.Current;

            container.Register(_envSettings);
            container.Register<ISetupWebDeploy, WebDeploySetup>().AsMultiInstance();
            container.Register<ISetupConDep, ConDepSetup>().AsMultiInstance();
            container.Register(_envSettings.LoadBalancer);
        }

        public static void Bootstrap(ConDepEnvironmentSettings envSettings)
        {
            new IoCBootstrapper(envSettings).BootstrapTinyIoC();
        }
    }
}
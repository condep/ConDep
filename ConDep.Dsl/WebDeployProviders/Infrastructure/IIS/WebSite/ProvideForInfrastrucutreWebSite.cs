using System;
using ConDep.Dsl;

namespace ConDep.Dsl
{
    public class ProvideForInfrastrucutreWebSite : IProvideOptions
    {
        ISetupWebDeploy IProvideOptions.WebDeploySetup { get; set; }
        Action<IProvide> IProvideOptions.AddProviderAction { get; set; }

        public string WebSiteName { get; set; }
        public string AppPoolName { get; set; }
    }
}
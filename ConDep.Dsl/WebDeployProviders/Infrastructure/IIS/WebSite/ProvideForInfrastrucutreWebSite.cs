using System;
using ConDep.Dsl.Model.Config;

namespace ConDep.Dsl.WebDeployProviders.Infrastructure.IIS.WebSite
{
    public class ProvideForInfrastrucutreWebSite : IProvideOptions
    {
        ISetupWebDeploy IProvideOptions.WebDeploySetup { get; set; }
        Action<IProvide> IProvideOptions.AddProviderAction { get; set; }
        IProvideConditions IProvideOptions.Condition { get; set; }
        CustomProviderConfig IProvideOptions.CustomConfig { get; set; }

        public string WebSiteName { get; set; }
        public string AppPoolName { get; set; }
    }
}
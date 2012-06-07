using System;
using ConDep.Dsl.Builders;
using ConDep.Dsl.Operations.WebDeploy.Model;

namespace ConDep.Dsl.Operations.WebDeploy.Options
{
    public interface IProviderCollection
    {
        void AddProvider(IProvide provider);
    }

    public interface IProvideForCustomIisDefinition : IProviderCollection
    {
    }

    public interface IProvideForExistingIisServer : IProviderCollection
    {
    }

    public interface IProvideForInfrastructureIis : IProviderCollection
    {
    }

    public interface IProvideForCustomWebSite : IProviderCollection
    {
        string WebSiteName { get; }
    }

    public interface IProvideForInfrastructure : IProviderCollection
    {
        IisDeploymentOptions IIS { get; }
        WindowsOptions Windows { get; }
    }

    public interface IProvideForDeployment : IProviderCollection
    {
        IisDeploymentOptions IIS { get; }
    }

    public class WindowsOptions
    {
        private readonly WebDeployDefinition _webDeployDefinition;

        public WindowsOptions(WebDeployDefinition webDeployDefinition)
        {
            _webDeployDefinition = webDeployDefinition;
        }

        public void InstallIIS()
        {
            _webDeployDefinition.Source.LocalHost = true;
            //iisDefinition(new ProviderOptions(_webDeployDefinition.Providers));
        }
    }
}
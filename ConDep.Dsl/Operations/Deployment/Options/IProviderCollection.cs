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

    public interface IProvideForCustomWebSite : IProviderCollection
    {
        string WebSiteName { get; }
    }

    public interface IProvideForDeployment : IProviderCollection
    {
        IisOptions IIS { get; }
    }
}
using System.Collections.Generic;
using ConDep.Dsl.Operations.WebDeploy.Model;

namespace ConDep.Dsl.Operations.WebDeploy.Options
{
    //public class ExistingServerProviderCollection
    //{
    //    private readonly List<IProvide> _providers;

    //    public ExistingServerProviderCollection(List<IProvide> providers)
    //    {
    //        _providers = providers;
    //    }

    //    protected internal void AddProvider(IProvide provider)
    //    {
    //        _providers.Add(provider);

    //        if(provider is CompositeProvider)
    //        {   
    //            ((CompositeProvider) provider).Configure();
    //        }
    //    }
    //}

    //public class CustomDefinitionProviderCollection
    //{
    //    private readonly List<IProvide> _providers;

    //    public CustomDefinitionProviderCollection(List<IProvide> providers)
    //    {
    //        _providers = providers;
    //    }

    //    protected internal void AddProvider(IProvide provider)
    //    {
    //        _providers.Add(provider);

    //        if (provider is CompositeProvider)
    //        {
    //            ((CompositeProvider)provider).Configure();
    //        }
    //    }
    //}

    public class ProviderCollection : IProvideForExistingIisServer, IProvideForCustomIis, IProvideForCustomWebSite, IProvideForServer
    {
        private readonly List<IProvide> _providers;

        public ProviderCollection(List<IProvide> providers)
        {
            _providers = providers;
        }

        public void AddProvider(IProvide provider)
        {
            _providers.Add(provider);

            if (provider is CompositeProvider)
            {
                ((CompositeProvider)provider).Configure();
            }
        }
    }

    public interface IProviderCollection
    {
        void AddProvider(IProvide provider);
    }

    public interface IProvideForCustomIis : IProviderCollection
    {
    }

    public interface IProvideForExistingIisServer : IProviderCollection
    {
    }

    public interface IProvideForCustomWebSite : IProviderCollection
    {
        
    }

    public interface IProvideForServer : IProviderCollection
    {
    }

}
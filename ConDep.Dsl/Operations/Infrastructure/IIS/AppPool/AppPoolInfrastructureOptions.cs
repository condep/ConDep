using ConDep.Dsl.Operations.Infrastructure.IIS.WebSite;

namespace ConDep.Dsl.Operations.Infrastructure.IIS.AppPool
{
    public class AppPoolInfrastructureOptions
    {
        private readonly IisAppPoolInfrastructureOperation _iisAppPoolProvider;
        private IdentityInfrastructureOptions _identityOptions;

        public AppPoolInfrastructureOptions(IisAppPoolInfrastructureOperation iisAppPoolProvider)
        {
            _iisAppPoolProvider = iisAppPoolProvider;
        }

        public void NetFrameworkVersion(NetFrameworkVersion netFrameworkVersion)
        {
            _iisAppPoolProvider.AppPoolOptions.NetFrameworkVersion = netFrameworkVersion;
        }

        public void ManagedPipeline(ManagedPipeline managedPipeline)
        {
            _iisAppPoolProvider.AppPoolOptions.ManagedPipeline = managedPipeline;
        }

        public IdentityInfrastructureOptions Identity
        {
            get { return _identityOptions ?? (_identityOptions = new IdentityInfrastructureOptions(_iisAppPoolProvider.AppPoolOptions)); }
        }

        public bool Enable32Bit
        {
            get { return _iisAppPoolProvider.AppPoolOptions.Enable32Bit.HasValue ? _iisAppPoolProvider.AppPoolOptions.Enable32Bit.Value : false; }
            set { _iisAppPoolProvider.AppPoolOptions.Enable32Bit = value; }
        }

        public int IdleTimeoutInMinutes
        {
            get { return _iisAppPoolProvider.AppPoolOptions.IdleTimeoutInMinutes.HasValue ? _iisAppPoolProvider.AppPoolOptions.IdleTimeoutInMinutes.Value : 0; }
            set { _iisAppPoolProvider.AppPoolOptions.IdleTimeoutInMinutes = value; }
        }

        public bool LoadUserProfile
        {
            get { return _iisAppPoolProvider.AppPoolOptions.LoadUserProfile.HasValue ? _iisAppPoolProvider.AppPoolOptions.LoadUserProfile.Value : false; }
            set { _iisAppPoolProvider.AppPoolOptions.LoadUserProfile = value; }
        }

        public int RecycleTimeIntervalInMinutes
        {
            get { return _iisAppPoolProvider.AppPoolOptions.RecycleTimeInMinutes.HasValue ? _iisAppPoolProvider.AppPoolOptions.RecycleTimeInMinutes.Value : 0; }
            set { _iisAppPoolProvider.AppPoolOptions.RecycleTimeInMinutes = value; }
        }
    }
}
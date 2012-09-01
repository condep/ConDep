using ConDep.Dsl.WebDeployProviders.Infrastructure.IIS.WebSite;

namespace ConDep.Dsl.WebDeployProviders.Infrastructure.IIS.AppPool
{
    public class AppPoolInfrastructureOptions
    {
        private readonly AppPoolInfrastructureProvider _appPoolProvider;
        private IdentityInfrastructureOptions _identityOptions;

        public AppPoolInfrastructureOptions(AppPoolInfrastructureProvider appPoolProvider)
        {
            _appPoolProvider = appPoolProvider;
        }

        public void NetFrameworkVersion(NetFrameworkVersion netFrameworkVersion)
        {
            _appPoolProvider.AppPool.NetFrameworkVersion = netFrameworkVersion;
        }

        public void ManagedPipeline(ManagedPipeline managedPipeline)
        {
            _appPoolProvider.AppPool.ManagedPipeline = managedPipeline;
        }

        public IdentityInfrastructureOptions Identity
        {
            get { return _identityOptions ?? (_identityOptions = new IdentityInfrastructureOptions(_appPoolProvider.AppPool)); }
        }

        public bool Enable32Bit
        {
            get { return _appPoolProvider.AppPool.Enable32Bit.HasValue ? _appPoolProvider.AppPool.Enable32Bit.Value : false; }
            set { _appPoolProvider.AppPool.Enable32Bit = value; }
        }

        public int IdleTimeoutInMinutes
        {
            get { return _appPoolProvider.AppPool.IdleTimeoutInMinutes.HasValue ? _appPoolProvider.AppPool.IdleTimeoutInMinutes.Value : 0; }
            set { _appPoolProvider.AppPool.IdleTimeoutInMinutes = value; }
        }

        public bool LoadUserProfile
        {
            get { return _appPoolProvider.AppPool.LoadUserProfile.HasValue ? _appPoolProvider.AppPool.LoadUserProfile.Value : false; }
            set { _appPoolProvider.AppPool.LoadUserProfile = value; }
        }

        public int RecycleTimeIntervalInMinutes
        {
            get { return _appPoolProvider.AppPool.RecycleTimeInMinutes.HasValue ? _appPoolProvider.AppPool.RecycleTimeInMinutes.Value : 0; }
            set { _appPoolProvider.AppPool.RecycleTimeInMinutes = value; }
        }
    }
}
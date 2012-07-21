using System;

namespace ConDep.Dsl
{
    public class AppPoolInfrastructureOptions
    {
        private readonly WebSiteInfrastructureProvider _webSiteInfrastructureProvider;
        private IdentityInfrastructureOptions _identityOptions;

        public AppPoolInfrastructureOptions(WebSiteInfrastructureProvider webSiteInfrastructureProvider)
        {
            _webSiteInfrastructureProvider = webSiteInfrastructureProvider;
        }

        public void NetFrameworkVersion(NetFrameworkVersion netFrameworkVersion)
        {
            _webSiteInfrastructureProvider.ApplicationPool.NetFrameworkVersion = netFrameworkVersion;
        }

        public void ManagedPipeline(ManagedPipeline managedPipeline)
        {
            _webSiteInfrastructureProvider.ApplicationPool.ManagedPipeline = managedPipeline;
        }

        public IdentityInfrastructureOptions Identity
        {
            get { return _identityOptions ?? (_identityOptions = new IdentityInfrastructureOptions(_webSiteInfrastructureProvider.ApplicationPool)); }
        }

        public bool Enable32Bit
        {
            get { return _webSiteInfrastructureProvider.ApplicationPool.Enable32Bit.HasValue ? _webSiteInfrastructureProvider.ApplicationPool.Enable32Bit.Value : false; }
            set { _webSiteInfrastructureProvider.ApplicationPool.Enable32Bit = value; }
        }

        public int IdleTimeoutInMinutes
        {
            get { return _webSiteInfrastructureProvider.ApplicationPool.IdleTimeoutInMinutes.HasValue ? _webSiteInfrastructureProvider.ApplicationPool.IdleTimeoutInMinutes.Value : 0; }
            set { _webSiteInfrastructureProvider.ApplicationPool.IdleTimeoutInMinutes = value; }
        }

        public bool LoadUserProfile
        {
            get { return _webSiteInfrastructureProvider.ApplicationPool.LoadUserProfile.HasValue ? _webSiteInfrastructureProvider.ApplicationPool.LoadUserProfile.Value : false; }
            set { _webSiteInfrastructureProvider.ApplicationPool.LoadUserProfile = value; }
        }

        public int RecycleTimeIntervalInMinutes
        {
            get { return _webSiteInfrastructureProvider.ApplicationPool.RecycleTimeInMinutes.HasValue ? _webSiteInfrastructureProvider.ApplicationPool.RecycleTimeInMinutes.Value : 0; }
            set { _webSiteInfrastructureProvider.ApplicationPool.RecycleTimeInMinutes = value; }
        }
    }
}
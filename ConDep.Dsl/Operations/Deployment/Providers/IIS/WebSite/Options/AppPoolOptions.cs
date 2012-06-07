using System;

namespace ConDep.Dsl
{
    public class AppPoolOptions
    {
        private readonly CustomWebSiteProvider _customWebSiteProvider;
        private IdentityOptions _identityOptions;

        public AppPoolOptions(CustomWebSiteProvider customWebSiteProvider)
        {
            _customWebSiteProvider = customWebSiteProvider;
        }

        public void NetFrameworkVersion(NetFrameworkVersion netFrameworkVersion)
        {
            _customWebSiteProvider.ApplicationPool.NetFrameworkVersion = netFrameworkVersion;
        }

        public void ManagedPipeline(ManagedPipeline managedPipeline)
        {
            _customWebSiteProvider.ApplicationPool.ManagedPipeline = managedPipeline;
        }

        public IdentityOptions Identity
        {
            get { return _identityOptions ?? (_identityOptions = new IdentityOptions(_customWebSiteProvider.ApplicationPool)); }
        }

        public bool Enable32Bit
        {
            get { return _customWebSiteProvider.ApplicationPool.Enable32Bit.HasValue ? _customWebSiteProvider.ApplicationPool.Enable32Bit.Value : false; }
            set { _customWebSiteProvider.ApplicationPool.Enable32Bit = value; }
        }

        public int IdleTimeoutInMinutes
        {
            get { return _customWebSiteProvider.ApplicationPool.IdleTimeoutInMinutes.HasValue ? _customWebSiteProvider.ApplicationPool.IdleTimeoutInMinutes.Value : 0; }
            set { _customWebSiteProvider.ApplicationPool.IdleTimeoutInMinutes = value; }
        }

        public bool LoadUserProfile
        {
            get { return _customWebSiteProvider.ApplicationPool.LoadUserProfile.HasValue ? _customWebSiteProvider.ApplicationPool.LoadUserProfile.Value : false; }
            set { _customWebSiteProvider.ApplicationPool.LoadUserProfile = value; }
        }

        public int RecycleTimeIntervalInMinutes
        {
            get { return _customWebSiteProvider.ApplicationPool.RecycleTimeInMinutes.HasValue ? _customWebSiteProvider.ApplicationPool.RecycleTimeInMinutes.Value : 0; }
            set { _customWebSiteProvider.ApplicationPool.RecycleTimeInMinutes = value; }
        }
    }
}
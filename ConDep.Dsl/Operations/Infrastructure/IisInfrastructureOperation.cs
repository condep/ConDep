using System.Collections.Generic;
using ConDep.Dsl.Builders;
using ConDep.Dsl.SemanticModel;

namespace ConDep.Dsl.Operations.Infrastructure
{
    public class IisInfrastructureOperation : RemoteCompositeOperation
    {
        private readonly List<string> _roleServicesToAdd = new List<string>();
        private readonly List<string> _roleServicesToRemove = new List<string>();

        public override void Configure(IOfferRemoteOperations server)
        {
            var removeFeatures = _roleServicesToRemove.Count > 0 ? "$featureRemoved = Remove-WindowsFeature " + string.Join(",", _roleServicesToRemove) : "";
            server.ExecuteRemote.PowerShell("Import-Module Servermanager; $feature = Add-WindowsFeature Web-Server,Web-WebServer," + string.Join(",", _roleServicesToAdd) + "; $feature; $feature.FeatureResult; " + removeFeatures, opt => opt.WaitIntervalInSeconds(5).RetryAttempts(100));
        }

        public override bool IsValid(Notification notification)
        {
            return true;
        }

        public void AddRoleService(string roleService)
        {
            _roleServicesToAdd.Add(roleService);            
        }

        public void RemoveRoleService(string roleService)
        {
            _roleServicesToRemove.Add(roleService);
        }
    }
}
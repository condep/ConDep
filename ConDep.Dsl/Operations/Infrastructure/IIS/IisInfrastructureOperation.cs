using System.Collections.Generic;
using ConDep.Dsl.SemanticModel;

namespace ConDep.Dsl.Operations.Infrastructure.IIS
{
    public class IisInfrastructureOperation : RemoteCompositeInfrastructureOperation
    {
        private readonly List<string> _roleServicesToAdd = new List<string>();
        private readonly List<string> _roleServicesToRemove = new List<string>();

        public override void Configure(IOfferRemoteComposition server, IOfferInfrastructure require)
        {
            var removeFeatures = _roleServicesToRemove.Count > 0 ? string.Join(",", _roleServicesToRemove) : "$null";
            var addFeatures = "Web-Server,Web-WebServer" + (_roleServicesToAdd.Count > 0 ? "," : "") + string.Join(",", _roleServicesToAdd);
            server.ExecuteRemote.PowerShell(string.Format("Set-ConDepWindowsFeatures {0} {1}", addFeatures, removeFeatures));
        }

        public override string Name
        {
            get { return "IIS Installer"; }
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
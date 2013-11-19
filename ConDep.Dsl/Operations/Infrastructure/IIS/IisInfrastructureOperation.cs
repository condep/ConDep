using System.Collections.Generic;
using ConDep.Dsl.SemanticModel;

namespace ConDep.Dsl.Operations.Infrastructure.IIS
{
    public class IisInfrastructureOperation : RemoteCompositeInfrastructureOperation
    {
        private readonly List<string> _featuresToAdd = new List<string>();
        private readonly List<string> _featuresToRemove = new List<string>();

        public override string Name
        {
            get { return "IIS Installer"; }
        }

        public override bool IsValid(Notification notification)
        {
            return true;
        }

        public override void Configure(IOfferRemoteComposition server, IOfferInfrastructure require)
        {
            require
                .Windows(win =>
                {
                    win.InstallFeature("Web-Server");
                    win.InstallFeature("Web-WebServer");

                    foreach (var feature in _featuresToAdd)
                    {
                        win.InstallFeature(feature);
                    }

                    foreach (var feature in _featuresToRemove)
                    {
                        win.UninstallFeature(feature);
                    }
                });
        }

        public void AddRoleService(string roleService)
        {
            _featuresToAdd.Add(roleService);
        }

        public void RemoveRoleService(string roleService)
        {
            _featuresToRemove.Add(roleService);
        }

    }
}
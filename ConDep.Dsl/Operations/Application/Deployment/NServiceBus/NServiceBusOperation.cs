using System;
using System.IO;
using ConDep.Dsl.Config;
using ConDep.Dsl.SemanticModel;

namespace ConDep.Dsl.Operations.Application.Deployment.NServiceBus
{
    public class NServiceBusOperation : RemoteCompositeOperation, IRequireCustomConfiguration
    {
        private string _serviceInstallerName = "NServiceBus.Host.exe";
        private string _sourcePath;
        private string _destPath;
        private readonly string _serviceName;
        private readonly string _profile;
        private readonly Action<IOfferWindowsServiceOptions> _options;

        public NServiceBusOperation(string path, string destDir, string serviceName, string profile, Action<IOfferWindowsServiceOptions> options)
        {
            _sourcePath = Path.GetFullPath(path);
            _destPath = destDir;
            _serviceName = serviceName;
            _profile = profile;
            _options = options;
        }

        public override void Configure(IOfferRemoteComposition server)
        {
            var installParams = string.Format("/install /serviceName:\"{0}\" /displayName:\"{0}\" {1}", _serviceName, _profile);
            server.Deploy.WindowsServiceWithInstaller(_serviceName, _serviceName, _sourcePath, _destPath, _serviceInstallerName,
                                                      installParams, _options);
        }

        public override string Name
        {
            get { return "NServiceBus"; }
        }

        public override bool IsValid(Notification notification)
        {
            return true;
        }
    }
}
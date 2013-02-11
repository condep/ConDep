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
        private readonly Action<IOfferWindowsServiceOptions> _options;

        public NServiceBusOperation(string path, string destDir, string serviceName, string profile, Action<IOfferWindowsServiceOptions> options)
        {
            _sourcePath = Path.GetFullPath(path);
            _destPath = destDir;
            _serviceName = serviceName;
            _options = options;
        }

        internal string Profile { get; set; }

        public override void Configure(IOfferRemoteComposition server)
        {
            var installParams = string.Format("/install /serviceName:\"{0}\" /displayName:\"{0}\" {1}", _serviceName, Profile);
            server.Deploy.WindowsServiceWithInstaller(_serviceName, _sourcePath, _destPath, _serviceInstallerName,
                                                      _serviceName, installParams, _options);
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
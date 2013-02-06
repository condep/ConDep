using ConDep.Dsl.Builders;
using ConDep.Dsl.SemanticModel;

namespace ConDep.Dsl.Operations.Application.Deployment.WindowsService
{
    public class WindowsServiceOperation : RemoteCompositeOperation
    {
        private readonly string _serviceName;
        private readonly string _sourceDir;
        private readonly string _destDir;
        private readonly string _relativeExePath;
        private readonly string _displayName;
        private readonly WindowsServiceOptions.WindowsServiceOptionValues _values;

        public WindowsServiceOperation(string serviceName, string sourceDir, string destDir, string relativeExePath, string displayName) : this(serviceName, sourceDir, destDir, relativeExePath, displayName, null)
        {
        }

        public WindowsServiceOperation(string serviceName, string sourceDir, string destDir, string relativeExePath, string displayName, WindowsServiceOptions.WindowsServiceOptionValues winServiceOptionValues)
        {
            _serviceName = serviceName;
            _sourceDir = sourceDir;
            _destDir = destDir;
            _relativeExePath = relativeExePath;
            _displayName = displayName;
            _values = winServiceOptionValues;
        }

        public override string Name
        {
            get { return "Windows Service"; }
        }

        public override bool IsValid(Notification notification)
        {
            return true;
        }

        public override void Configure(IOfferRemoteComposition server)
        {
            server.Deploy.Directory(_sourceDir, _destDir);
            server.ExecuteRemote.PowerShell("");
        }
    }
}
namespace ConDep.Dsl.SemanticModel
{
    public class ConDepOptions
    {
        private readonly bool _deployAllApps;
        private readonly string _application;
        private readonly bool _deployOnly;
        private readonly bool _infraOnly;
        private readonly bool _webDeployExist;
        private readonly bool _stopAfterMarkedServer;
        private readonly bool _continueAfterMarkedServer;

        public ConDepOptions(bool deployAllApps, string application, bool deployOnly, bool infraOnly, bool webDeployExist, bool stopAfterMarkedServer, bool continueAfterMarkedServer)
        {
            _deployAllApps = deployAllApps;
            _application = application;
            _deployOnly = deployOnly;
            _infraOnly = infraOnly;
            _webDeployExist = webDeployExist;
            _stopAfterMarkedServer = stopAfterMarkedServer;
            _continueAfterMarkedServer = continueAfterMarkedServer;
        }

        public string Application { get { return string.IsNullOrWhiteSpace(_application) ? "Default" : _application; } }

        public bool DeployOnly
        {
            get { return _deployOnly; }
        }

        public bool InfraOnly
        {
            get { return _infraOnly; }
        }

        public bool WebDeployExist
        {
            get { return _webDeployExist; }
        }

        public bool StopAfterMarkedServer
        {
            get { return _stopAfterMarkedServer; }
        }

        public bool ContinueAfterMarkedServer
        {
            get { return _continueAfterMarkedServer; }
        }

        public bool DeployAllApps
        {
            get { return _deployAllApps; }
        }

        public bool HasApplicationDefined()
        {
            return !string.IsNullOrWhiteSpace(Application) && Application != "Default";
        }
    }
}
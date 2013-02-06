using System.Collections.Generic;
using System.Reflection;

namespace ConDep.Dsl.SemanticModel
{
    public class ConDepOptions
    {
        private readonly bool _deployAllApps;
        private readonly string _application;
        private readonly bool _deployOnly;
        private readonly bool _webDeployExist;
        private readonly bool _stopAfterMarkedServer;
        private readonly bool _continueAfterMarkedServer;
        private readonly Assembly _assembly;

        public ConDepOptions(bool deployAllApps, string application, bool deployOnly, bool webDeployExist, bool stopAfterMarkedServer, bool continueAfterMarkedServer, Assembly assembly)
        {
            _deployAllApps = deployAllApps;
            _application = application;
            _deployOnly = deployOnly;
            _webDeployExist = webDeployExist;
            _stopAfterMarkedServer = stopAfterMarkedServer;
            _continueAfterMarkedServer = continueAfterMarkedServer;
            _assembly = assembly;
        }

        public string Application { get { return string.IsNullOrWhiteSpace(_application) ? "Default" : _application; } }

        public bool DeployOnly
        {
            get { return _deployOnly; }
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

        public Assembly Assembly
        {
            get { return _assembly; }
        }

        public bool HasApplicationDefined()
        {
            return !string.IsNullOrWhiteSpace(Application) && Application != "Default";
        }
    }
}
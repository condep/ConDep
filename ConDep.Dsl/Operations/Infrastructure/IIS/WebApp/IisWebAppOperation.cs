using System.Collections.Generic;
using ConDep.Dsl.Builders;
using ConDep.Dsl.Operations.Application.Execution.PowerShell;
using ConDep.Dsl.Resources;
using ConDep.Dsl.Scripts;
using ConDep.Dsl.SemanticModel;

namespace ConDep.Dsl.Operations.Infrastructure.IIS.WebApp
{
    public class IisWebAppOperation : RemoteCompositeInfrastructureOperation, IRequireRemotePowerShellScript
    {
        private readonly string _webAppName;
        private readonly string _webSiteName;
        private readonly IisWebAppOptions.IisWebAppOptionsValues _options;
        private List<string> _scriptPaths = new List<string>();

        public IisWebAppOperation(string webAppName, string webSiteName)
        {
            _webAppName = webAppName;
            _webSiteName = webSiteName;
        }

        public IisWebAppOperation(string webAppName, string webSiteName, IisWebAppOptions.IisWebAppOptionsValues options)
        {
            _webAppName = webAppName;
            _webSiteName = webSiteName;
            _options = options;
        }

        public override void Configure(IOfferRemoteComposition server, IOfferInfrastructure require)
        {
            server.ExecuteRemote.PowerShell(string.Format(@"Import-Module $env:temp\ConDepPowerShellScripts\ConDep; New-ConDepWebApp '{0}' '{1}' {2} {3};"
                , _webAppName
                , _webSiteName
                , (_options == null || string.IsNullOrWhiteSpace(_options.PhysicalPath)) ? "$null" : "'" + _options.PhysicalPath + "'"
                , (_options == null || string.IsNullOrWhiteSpace(_options.AppPool)) ? "$null" : "'" + _options.AppPool + "'")
            , psOptions => psOptions.WaitIntervalInSeconds(30));
        }

        public override string Name
        {
            get { return "Web Application"; }
        }

        public override bool IsValid(Notification notification)
        {
            return !string.IsNullOrWhiteSpace(_webAppName) 
                && !string.IsNullOrWhiteSpace(_webSiteName);
        }

        public IEnumerable<string> ScriptPaths
        {
            get
            {
                if (_scriptPaths.Count == 0)
                {
                    _scriptPaths.Add(ConDepResourceFiles.GetFilePath(typeof(ScriptNamespaceMarker).Namespace, "Iis.ps1", true));
                    _scriptPaths.Add(ConDepResourceFiles.GetFilePath(typeof(ScriptNamespaceMarker).Namespace, "ConDep.psm1", true));
                }
                return _scriptPaths;
            }
        }
    }
}
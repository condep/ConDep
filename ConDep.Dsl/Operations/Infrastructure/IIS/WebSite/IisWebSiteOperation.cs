using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using ConDep.Dsl.Builders;
using ConDep.Dsl.Config;
using ConDep.Dsl.Operations.Application.Execution.PowerShell;
using ConDep.Dsl.Resources;
using ConDep.Dsl.Scripts;
using ConDep.Dsl.SemanticModel;

namespace ConDep.Dsl.Operations.Infrastructure.IIS.WebSite
{
    public class IisWebSiteOperation : RemoteCompositeOperation, IRequireCustomConfiguration, IRequireRemotePowerShellScript
    {
        private readonly string _webSiteName;
        private readonly int _id;
        private readonly IisWebSiteOptions _options;
        private List<string> _scriptPaths = new List<string>();

        public IisWebSiteOperation(string webSiteName, int id)
        {
            _webSiteName = webSiteName;
            _id = id;
            _options = new IisWebSiteOptions();
        }

        public IisWebSiteOperation(string webSiteName, int id, IisWebSiteOptions options)
        {
            _webSiteName = webSiteName;
            _id = id;
            _options = options;
        }

        public override string Name
        {
            get { return "IIS Web Site"; }
        }

        public override bool IsValid(Notification notification)
        {
            return !string.IsNullOrWhiteSpace(_webSiteName);
        }

        public override void Configure(IOfferRemoteComposition server)
        {
            var bindingScript = "";
            foreach(var httpBinding in _options.Values.HttpBindings)
            {
                bindingScript += string.Format("New-ConDepIisHttpBinding '{0}' '{1}' '{2}' '{3}'; ", _webSiteName, httpBinding.Port, httpBinding.Ip, httpBinding.HostName);
            }

            foreach (var httpsBinding in _options.Values.HttpsBindings)
            {
                switch (httpsBinding.CertLocation)
                {
                    case CertLocation.Store:
                        server.Deploy.SslCertificate.FromStore(httpsBinding.FindType, httpsBinding.FindName);
                        break;

                    case CertLocation.File:
                        server.Deploy.SslCertificate.FromFile(httpsBinding.FilePath, httpsBinding.PrivateKeyPassword);
                        break;

                    default:
                        throw new Exception();
                }

                var type = httpsBinding.FindType.GetType();
                var findType = string.Format("[{0}]::{1}", type.FullName, httpsBinding.FindType);

                if(httpsBinding.FindType == X509FindType.FindByThumbprint)
                {
                    httpsBinding.FindName = httpsBinding.FindName.Replace(" ", "");
                }
                bindingScript += string.Format("New-ConDepIisHttpsBinding '{0}' {1} '{2}' '{3}' '{4}' '{5}'; ", _webSiteName, httpsBinding.FindType, httpsBinding.FindName, httpsBinding.BindingOptions.Port, httpsBinding.BindingOptions.Ip, httpsBinding.BindingOptions.HostName);
            }

            server.ExecuteRemote.PowerShell(string.Format(@"Import-Module $env:temp\ConDepPowerShellScripts\ConDep; New-ConDepIisWebSite '{0}' {1} {2} '{3}'; {4}"
                , _webSiteName
                , _id
                , (string.IsNullOrWhiteSpace(_options.Values.PhysicalPath) ? "$null" : "'" + _options.Values.PhysicalPath + "'")
                , _options.Values.AppPool
                , bindingScript)
                , o => o.WaitIntervalInSeconds(2).RetryAttempts(20));
        }

        public IEnumerable<string> ScriptPaths
        {
            get
            {
                if(_scriptPaths.Count == 0)
                {
                    _scriptPaths.Add(ConDepResourceFiles.GetFilePath(typeof(ScriptNamespaceMarker).Namespace, "Iis.ps1", true));
                    _scriptPaths.Add(ConDepResourceFiles.GetFilePath(typeof(ScriptNamespaceMarker).Namespace, "ConDep.psm1", true));
                }
                return _scriptPaths;
            }
        }
    }
}
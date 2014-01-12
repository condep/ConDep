using System.Linq;
using System.Security.Cryptography.X509Certificates;
using ConDep.Dsl.Config;
using ConDep.Dsl.SemanticModel;

namespace ConDep.Dsl.Operations.Infrastructure.IIS.WebSite
{
    public class IisWebSiteOperation : RemoteCompositeInfrastructureOperation, IRequireCustomConfiguration
    {
        private readonly string _webSiteName;
        private readonly int _id;
        private readonly IisWebSiteOptions _options;

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

        public override void Configure(IOfferRemoteComposition server, IOfferInfrastructure require)
        {
            var bindings = _options.Values.HttpBindings.Select(httpBinding => string.Format("@{{protocol='http';bindingInformation='{0}:{1}:{2}'}}", httpBinding.Ip, httpBinding.Port, httpBinding.HostName)).ToList();

            foreach (var httpsBinding in _options.Values.HttpsBindings)
            {
                if (httpsBinding.FindType == X509FindType.FindByThumbprint)
                {
                    httpsBinding.FindName = httpsBinding.FindName.Replace(" ", "");
                }
                var type = httpsBinding.FindType.GetType();
                bindings.Add(string.Format("@{{protocol='https';bindingInformation='{0}:{1}:{2}';findType=[{3}]::{4};findValue='{5}'}}", httpsBinding.BindingOptions.Ip, httpsBinding.BindingOptions.Port, httpsBinding.BindingOptions.HostName, type.FullName, httpsBinding.FindType, httpsBinding.FindName));
            }

            server.ExecuteRemote.PowerShell(string.Format(@"New-ConDepIisWebSite '{0}' {1} {2} {3} '{4}' '{5}';"
                , _webSiteName
                , _id
                , "@(" + string.Join(",", bindings) + ")"
                , (string.IsNullOrWhiteSpace(_options.Values.PhysicalPath) ? "$null" : "'" + _options.Values.PhysicalPath + "'")
                , _options.Values.AppPool
                , _options.Values.LogDirectory));

            foreach(var webApp in _options.Values.WebApps)
            {
                require.IISWebApp(webApp.Item1, _webSiteName, webApp.Item2);
            }
        }
    }
}
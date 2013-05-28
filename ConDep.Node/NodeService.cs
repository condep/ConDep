using System;
using System.Diagnostics;
using System.ServiceModel;
using System.ServiceProcess;
using System.Web.Http;
using System.Web.Http.SelfHost;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace ConDep.Node
{
    partial class NodeService : ServiceBase
    {
        private HttpSelfHostServer _server;
        private HttpSelfHostConfiguration _config;

        public NodeService(string url)
        {
            InitializeComponent();

            this.CanHandlePowerEvent = false;
            this.CanHandleSessionChangeEvent = false;
            this.CanPauseAndContinue = false;
            this.CanShutdown = false;
            this.CanStop = true;
            this.EventLog.EnableRaisingEvents = true;
            this.EventLog.Source = "ConDepNode";
            this.EventLog.Log = "Application";

            AppDomain.CurrentDomain.UnhandledException += (sender, args) => EventLog.WriteEntry("Error: " + args.ExceptionObject.ToString(), EventLogEntryType.Error);
            var uri = new Uri(url);
            _config = new HttpSelfHostConfiguration(uri)
            {
                TransferMode = TransferMode.Streamed,
                MaxReceivedMessageSize = 2147483648,
                //ClientCredentialType = HttpClientCredentialType.Windows
                //MaxReceivedMessageSize = 2000000
            };

            var serializerSettings = _config.Formatters.JsonFormatter.SerializerSettings;
            serializerSettings.Formatting = Formatting.Indented;
            serializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            _config.Routes.MapHttpRoute("Sync", "api/sync/{controller}");
            _config.Routes.MapHttpRoute("WebAppSync", "api/sync/webapp/{siteName}/{appName}", new { controller = "WebApp" });
            _config.Routes.MapHttpRoute("Iis", "api/iis/{siteName}/{appName}", new { controller = "Iis", siteName = RouteParameter.Optional, appName = RouteParameter.Optional });
            _config.Routes.MapHttpRoute("Api", "api/{controller}", new { controller = "Home" });
            //config.Routes.MapHttpRoute("Iis", "api/{controller}", new { controller = "Iis" });
            //config.Routes.MapHttpRoute("WebApp", "api/iis/{controller}", new { controller = "IisWebApp" });
            //config.Routes.MapHttpRoute("Default", "api/{controller}/{id}", new { id = RouteParameter.Optional });
            //config.Routes.MapHttpRoute("File Upload", "api/sync/{controller}/{filename}");
        }

        protected override void OnStart(string[] args)
        {
            _server = new HttpSelfHostServer(_config);
            _server.OpenAsync();
            EventLog.WriteEntry("ConDepNode started", EventLogEntryType.Information);
        }

        protected override void OnStop()
        {
            _server.CloseAsync().Wait();
            _server.Dispose();
            EventLog.WriteEntry("ConDepNode stopped", EventLogEntryType.Information);
        }
    }
}

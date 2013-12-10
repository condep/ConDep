using System;
using System.Diagnostics;
using System.ServiceProcess;
using System.Web.Http;
using System.Web.Http.SelfHost;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Raven.Abstractions.Data;

namespace ConDep.Server
{
    partial class ConDepServer : ServiceBase
    {
        private HttpSelfHostServer _server;
        private HttpSelfHostConfiguration _config;
        private static QueueExecutor _queueExecutor;

        public ConDepServer(string url)
        {
            InitializeComponent();
            ConfigureService();
            ConfigureWebApi(url);

            _queueExecutor = new QueueExecutor(EventLog);
        }

        private void ConfigureWebApi(string url)
        {
            var uri = new Uri(url);
            _config = new HttpSelfHostConfiguration(uri);
            var serializerSettings = _config.Formatters.JsonFormatter.SerializerSettings;
            serializerSettings.Formatting = Formatting.Indented;
            serializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            _config.Routes.MapHttpRoute("Logs", "api/logs/{env}/{id}", new { controller = "logs", env = RouteParameter.Optional, id = RouteParameter.Optional });
            _config.Routes.MapHttpRoute("Default", "api/{controller}/{id}", new { id = RouteParameter.Optional });
        }

        private void ConfigureService()
        {
            CanHandlePowerEvent = false;
            CanHandleSessionChangeEvent = false;
            CanPauseAndContinue = false;
            CanShutdown = false;
            CanStop = true;
            EventLog.EnableRaisingEvents = true;
            EventLog.Source = "ConDepServer";
            EventLog.Log = "Application";

            AppDomain.CurrentDomain.UnhandledException += (sender, args) => EventLog.WriteEntry("Error: " + args.ExceptionObject.ToString(), EventLogEntryType.Error);
        }

        public static QueueExecutor QueueExecutor
        {
            get { return _queueExecutor; }
        }

        protected override void OnStart(string[] args)
        {
            StartWebServer();
            StartRavenDb();
            _queueExecutor.EvaluateForExecution();
        }

        private void StartRavenDb()
        {
            RavenDb.DocumentStore.Initialize();
            RavenDb.CreateIndexes();
            RavenDb.DocumentStore.Changes()
                         .ForDocumentsStartingWith("execution_queue")
                         .Subscribe(change =>
                             {
                                 if (change.Type == DocumentChangeTypes.Put)
                                 {
                                     _queueExecutor.EvaluateForExecution();
                                 }
                             });
        }

        private void StartWebServer()
        {
            _server = new HttpSelfHostServer(_config);
            _server.OpenAsync();
            EventLog.WriteEntry("ConDep Server started", EventLogEntryType.Information);
        }

        protected override void OnStop()
        {
            _queueExecutor.Cancel();

            RavenDb.DocumentStore.Dispose();
            //EventLog.WriteEntry("ConDep Data Store stopped", EventLogEntryType.Information);

            EventLog.WriteEntry("About to stop ConDep Server", EventLogEntryType.Information);
            _server.CloseAsync().Wait();
            _server.Dispose();
            EventLog.WriteEntry("ConDep Server stopped", EventLogEntryType.Information);
        }
    }
}

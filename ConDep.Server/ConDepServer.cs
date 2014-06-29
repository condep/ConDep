using System;
using System.Diagnostics;
using System.ServiceProcess;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.SelfHost;
using ConDep.Server.Domain.Infrastructure;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Raven.Abstractions.Data;
using StructureMap;

namespace ConDep.Server
{
    partial class ConDepServer : ServiceBase
    {
        private HttpSelfHostServer _server;
        private HttpSelfHostConfiguration _config;

        public ConDepServer(string url)
        {
            InitializeComponent();

            ConfigureService();
            ConfigureWebApi(url);
        }

        private void ConfigureWebApi(string url)
        {
            var uri = new Uri(url);
            _config = new HttpSelfHostConfiguration(uri) { DependencyResolver = new StructureMapDependencyResolver(ObjectFactory.Container) };

            var serializerSettings = _config.Formatters.JsonFormatter.SerializerSettings;
            serializerSettings.Formatting = Formatting.Indented;
            serializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            _config.Formatters.Remove(_config.Formatters.XmlFormatter);
            
            _config.MapHttpAttributeRoutes();
            _config.EnableCors(new EnableCorsAttribute("http://localhost:1337", "*", "*"));
            
            _config.Routes.MapHttpRoute("DeployApplication", "api/deployment/application/{appName}", new { controller = "applicationdeployment" });
            _config.Routes.MapHttpRoute("Logs", "api/logs/{env}", new { controller = "logs", env = RouteParameter.Optional });
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

            AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
                {
                    EventLog.WriteEntry("Error: " + args.ExceptionObject.ToString(), EventLogEntryType.Error);
                };
        }

#if(DEBUG)
        public void Start(string[] args)
        {
            OnStart(args);

            while (true)
            {
                Thread.Sleep(1);
            }
        }
#endif
        protected override void OnStart(string[] args)
        {
            IoCBootStrapper.Bootstrap();
            EventDispatcher.AutoRegister();

            StartRavenDb();
            StartWebServer();
        }

        private void HandleAggregateException(Task task)
        {
            if (task.Exception != null)
            {
                    task.Exception.Handle(inner =>
                        {
                            EventLog.WriteEntry("Error: " + inner.ToString(), EventLogEntryType.Error);
                            return true;
                        });
            } 
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
                                     //_queueExecutor.EvaluateForExecution().ContinueWith(HandleAggregateException, TaskContinuationOptions.OnlyOnFaulted);
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
            //_queueExecutor.Cancel();

            RavenDb.DocumentStore.Dispose();
            //EventLog.WriteEntry("ConDep Data Store stopped", EventLogEntryType.Information);

            EventLog.WriteEntry("About to stop ConDep Server", EventLogEntryType.Information);
            _server.CloseAsync().Wait();
            _server.Dispose();
            EventLog.WriteEntry("ConDep Server stopped", EventLogEntryType.Information);
        }
    }
}

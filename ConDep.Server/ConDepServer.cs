using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.ServiceProcess;
using System.Text;
using System.Web.Http;
using System.Web.Http.SelfHost;
using ConDep.Dsl.Config;
using ConDep.Server.Api.Controllers;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Raven.Abstractions.Util;
using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Embedded;
using Raven.Database.Server;

namespace ConDep.Server
{
    partial class ConDepServer : ServiceBase
    {
        private HttpSelfHostServer _server;
        private HttpSelfHostConfiguration _config;

        public ConDepServer(string url)
        {
            InitializeComponent();

            this.CanHandlePowerEvent = false;
            this.CanHandleSessionChangeEvent = false;
            this.CanPauseAndContinue = false;
            this.CanShutdown = false;
            this.CanStop = true;
            this.EventLog.EnableRaisingEvents = true;
            this.EventLog.Source = "ConDepServer";
            this.EventLog.Log = "Application";

            AppDomain.CurrentDomain.UnhandledException += (sender, args) => EventLog.WriteEntry("Error: " + args.ExceptionObject.ToString(), EventLogEntryType.Error);
            var uri = new Uri(url);
            _config = new HttpSelfHostConfiguration(uri);
            //{
            //    MaxReceivedMessageSize = 2147483648
            //};

            var serializerSettings = _config.Formatters.JsonFormatter.SerializerSettings;
            serializerSettings.Formatting = Formatting.Indented;
            serializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            _config.Routes.MapHttpRoute("Default", "api/{controller}/{id}", new {id = RouteParameter.Optional});
            //_config.Routes.MapHttpRoute("WebAppSync", "api/sync/webapp/{siteName}/{appName}", new { controller = "WebApp" });
            //_config.Routes.MapHttpRoute("Iis", "api/iis/{siteName}/{appName}", new { controller = "Iis", siteName = RouteParameter.Optional, appName = RouteParameter.Optional });
            //_config.Routes.MapHttpRoute("Api", "api/{controller}", new { controller = "Home" });
        }

        protected override void OnStart(string[] args)
        {
            _server = new HttpSelfHostServer(_config);
            _server.OpenAsync();
            EventLog.WriteEntry("ConDep Server started", EventLogEntryType.Information);

            //_documentStore = new EmbeddableDocumentStore
            //{
            //    DataDirectory = "ConDepData",
            //    UseEmbeddedHttpServer = true, 
            //};
            DocumentStore.Initialize();
            //EventLog.WriteEntry("ConDep Data Store started", EventLogEntryType.Information);
        }

        protected override void OnStop()
        {
            DocumentStore.Dispose();
            //EventLog.WriteEntry("ConDep Data Store stopped", EventLogEntryType.Information);

            EventLog.WriteEntry("About to stop ConDep Server", EventLogEntryType.Information);
            _server.CloseAsync().Wait();
            _server.Dispose();
            EventLog.WriteEntry("ConDep Server stopped", EventLogEntryType.Information);
        }

        private static readonly Lazy<IDocumentStore> docStore = new Lazy<IDocumentStore>(() =>
        {
            var docStore = new EmbeddableDocumentStore
            {
                DataDirectory = "ConDepData",
                UseEmbeddedHttpServer = true
            };
            docStore.Conventions.RegisterIdConvention<ConDepEnvConfig>((dbname, commands, config) => "environments/" + config.EnvironmentName);
            docStore.Conventions.RegisterAsyncIdConvention<ConDepEnvConfig>((dbname, commands, config) => new CompletedTask<string>("environments/" + config.EnvironmentName));

            docStore.Conventions.RegisterIdConvention<ExecutionLog>((dbname, commands, log) => "log/" + log.ExecId);
            docStore.Conventions.RegisterAsyncIdConvention<ExecutionLog>((dbname, commands, log) => new CompletedTask<string>("log/" + log.ExecId));

            //OPTIONAL:
            //IndexCreation.CreateIndexes(typeof(Global).Assembly, docStore);

            return docStore;
        });

        public static IDocumentStore DocumentStore
        {
            get { return docStore.Value; }
        }
    }
}

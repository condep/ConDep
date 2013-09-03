using System;
using System.Collections.Generic;
using System.Diagnostics;
using ConDep.Dsl;
using ConDep.Dsl.Builders;
using ConDep.Dsl.Config;
using ConDep.Dsl.Logging;
using ConDep.Dsl.Operations.LoadBalancer;
using ConDep.Dsl.Remote;
using ConDep.Dsl.SemanticModel;
using ConDep.Dsl.SemanticModel.Sequence;
using ConDep.Dsl.SemanticModel.WebDeploy;
using ScriptCs.Contracts;
using log4net.Config;

namespace ScriptCs.ConDep
{
    public class ConDepPack : IScriptPackContext
    {
        private readonly ExecutionSequenceManager _sequenceManager;

        public ConDepPack()
        {
            _sequenceManager = new ExecutionSequenceManager(new DefaultLoadBalancer());

            ConfigureLogging();
        }

        private void ConfigureLogging()
        {
            var type = GetType();
            using (var logConfigStream = type.Module.Assembly.GetManifestResourceStream(type.Namespace + ".internal.condep.log4net.xml"))
            {
                XmlConfigurator.Configure(logConfigStream);
            }
        }

        public ConDepPack ConfigureApplication(string appName, ServerConfig server, Action<IOfferLocalOperations> action)
        {
            action(new LocalOperationsBuilder(_sequenceManager.NewLocalSequence(appName), new InfrastructureSequence(), new [] {server}));
            return this;
        }

        public IOfferLocalOperations ConfigureApplication(string appName, ServerConfig server)
        {
            return new LocalOperationsBuilder(_sequenceManager.NewLocalSequence(appName), new InfrastructureSequence(), new [] {server});
        }

        public IOfferInfrastructure ConfigureInfrastructure()
        {
            return new InfrastructureBuilder(new InfrastructureSequence());
        }

        public void Execute()
        {
            var clientValidator = new ClientValidator();
            clientValidator.Validate();

            //var serverInfoHarvester = new ServerInfoHarvester(conDepSettings);
            //var serverValidator = new RemoteServerValidator(conDepSettings.Config.Servers, serverInfoHarvester);

            var settings = new ConDepSettings();
            var server = new ServerConfig {Name = "127.0.0.1"};
            settings.Config.Servers= new List<ServerConfig>{server};

            _sequenceManager.Execute(new ConDepStatus(), settings);
        }
    }

    public class Test
    {
        public void TestThat()
        {
            var condep = new ConDepPack();
            var serverConf = new ServerConfig {Name = "127.0.0.1"};

            condep.ConfigureApplication("MyApp", serverConf, app => app
                .ToEachServer(server =>
                {
                    server.Deploy.Directory(@"C:\temp2", @"C:\temp3");
                    server.ExecuteRemote.PowerShell("echo 'You rock!!!'");
                })
            );
        }
    }
}
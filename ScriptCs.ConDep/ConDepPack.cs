using System;
using System.Collections.Generic;
using ConDep.Dsl;
using ConDep.Dsl.Builders;
using ConDep.Dsl.Config;
using ConDep.Dsl.Operations.LoadBalancer;
using ConDep.Dsl.Remote;
using ConDep.Dsl.SemanticModel;
using ConDep.Dsl.SemanticModel.Sequence;
using ScriptCs.Contracts;
using log4net.Config;

namespace ScriptCs.ConDep
{
    public class ConDepPack : IScriptPackContext
    {
        private readonly ExecutionSequenceManager _sequenceManager;
        private readonly InfrastructureSequence _infraSequence;
        private ConDepSettings _settings;

        public ConDepPack()
        {
            _sequenceManager = new ExecutionSequenceManager(new DefaultLoadBalancer());
            _infraSequence = new InfrastructureSequence();
            _settings = new ConDepSettings();

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
            _settings.Config.Servers = new List<ServerConfig>{server};
            action(new LocalOperationsBuilder(_sequenceManager.NewLocalSequence(appName), new InfrastructureSequence(), _settings.Config.Servers));
            return this;
        }

        //public ConDepPack WithInfrastructure(string scriptFile)
        //{
            
        //}

        //public ConDepPack ConfigureApplication(string appName, Action<IOfferLocalOperations> action)
        //{

        //    action(new LocalOperationsBuilder(_sequenceManager.NewLocalSequence(appName), new InfrastructureSequence(), new[] { server }));
        //    return this;
        //}

        //public IOfferLocalOperations ConfigureApplication(string appName, ServerConfig server)
        //{
        //    return new LocalOperationsBuilder(_sequenceManager.NewLocalSequence(appName), new InfrastructureSequence(), new [] {server});
        //}

        public IOfferInfrastructure ConfigureInfrastructure()
        {
            return new InfrastructureBuilder(new InfrastructureSequence());
        }

        public void Execute()
        {
            var clientValidator = new ClientValidator();
            var serverInfoHarvester = new ServerInfoHarvester(_settings);
            var serverValidator = new RemoteServerValidator(_settings.Config.Servers, serverInfoHarvester);

            new ConDepConfigurationExecutor().Execute(_settings, clientValidator, serverValidator, _sequenceManager);
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
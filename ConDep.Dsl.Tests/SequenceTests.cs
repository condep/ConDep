using ConDep.Dsl.Builders;
using ConDep.Dsl.Config;
using ConDep.Dsl.Operations.LoadBalancer;
using ConDep.Dsl.SemanticModel;
using ConDep.Dsl.SemanticModel.Sequence;
using NUnit.Framework;

namespace ConDep.Dsl.Tests
{
    [TestFixture]
    public class SequenceTests
    {
        private ExecutionSequenceManager _sequenceManager;
        private SequenceTestApp _app;
        private SequenceTestInfrastructure _infra;

        [SetUp]
        public void Setup()
        {
            _sequenceManager = new ExecutionSequenceManager(new DefaultLoadBalancer());
            _app = new SequenceTestApp();
            _infra = new SequenceTestInfrastructure();

        }

        [Test]
        public void TestThatExecutionSequenceIsValid()
        {
            var config = new ConDepEnvConfig {EnvironmentName = "bogusEnv"};
            var server = new ServerConfig { Name = "jat-web03" };
            config.Servers = new[] { server };

            var settings = new ConDepSettings();
            settings.Config = config;

            var infrastructureSequence = new InfrastructureSequence();
            var preOpsSequence = new PreOpsSequence();

            var infrastructureBuilder = new InfrastructureBuilder(infrastructureSequence);
            _infra.Configure(infrastructureBuilder, settings);

            var local = new LocalOperationsBuilder(_sequenceManager.NewLocalSequence("Test"), infrastructureSequence, preOpsSequence, config.Servers);
            _app.Configure(local, settings);

            var notification = new Notification();
            Assert.That(_sequenceManager.IsValid(notification));
        }
    }

    public class SequenceTestApp : ApplicationArtifact, IDependOnInfrastructure<SequenceTestInfrastructure>
    {
        public override void Configure(IOfferLocalOperations local, ConDepSettings settings)
        {
            local.HttpGet("http://www.con-dep.net");
            local.ToEachServer(server => server.ExecuteRemote.PowerShell("ipconfig"));
            local.HttpGet("http://blog.torresdal.net");
        }
    }

    public class SequenceTestInfrastructure : InfrastructureArtifact
    {
        public override void Configure(IOfferInfrastructure require, ConDepSettings settings)
        {
            require.IIS();
        }
    }
}
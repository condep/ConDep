using ConDep.Dsl.Builders;
using ConDep.Dsl.Config;
using ConDep.Dsl.Operations;
using ConDep.Dsl.Operations.LoadBalancer;
using ConDep.Dsl.SemanticModel.Sequence;
using NUnit.Framework;

namespace ConDep.Dsl.Tests
{
    [TestFixture]
    public class OnlyIfTests
    {
        private ExecutionSequenceManager _sequenceManager;
        private ServerInfo _serverInfo;

        [SetUp]
        public void Setup()
        {
            _sequenceManager = new ExecutionSequenceManager(new DefaultLoadBalancer());
            var app = new OnlyIfTestApp();
            var infra = new OnlyIfTestInfra();

            var config = new ConDepEnvConfig { EnvironmentName = "bogusEnv" };
            var server = new ServerConfig { Name = "bogusHost" };
            config.Servers = new[] { server };

            var settings = new ConDepSettings { Config = config };

            var infrastructureSequence = new InfrastructureSequence();
            var infrastructureBuilder = new InfrastructureBuilder(infrastructureSequence);

            infra.Configure(infrastructureBuilder, settings);
            var local = new LocalOperationsBuilder(_sequenceManager.NewLocalSequence("Test"), infrastructureSequence, config.Servers);
            app.Configure(local, settings);

            _serverInfo = new ServerInfo {OperatingSystem = new OperatingSystemInfo {Name = "Windows Server 2012"}};
        }

        [Test]
        public void TestThat_SequenceManagerHasOnlyOneLocalSequence()
        {
            Assert.That(_sequenceManager._sequence.Count, Is.EqualTo(1));
            Assert.That(_sequenceManager._sequence[0], Is.TypeOf<LocalSequence>());
        }

        [Test]
        public void TestThat_LocalSequenceOnlyHaveOneRemoteSequence()
        {
            var seq = _sequenceManager._sequence[0];
            Assert.That(seq._sequence.Count, Is.EqualTo(1));
            Assert.That(seq._sequence[0], Is.TypeOf<RemoteSequence>());
        }

        [Test]
        public void TestThat_RemoteSequenceOnlyHaveOneConditionalSequence()
        {
            var seq = _sequenceManager._sequence[0]._sequence[0] as RemoteSequence;

            Assert.That(seq._sequence.Count, Is.EqualTo(1));
            Assert.That(seq._sequence[0], Is.TypeOf<CompositeConditionalSequence>());
        }

        [Test]
        public void TestThat_ConditionalSequenceOnlyHaveOneOperation()
        {
            var remSeq = _sequenceManager._sequence[0]._sequence[0] as RemoteSequence;
            var seq = remSeq._sequence[0] as CompositeConditionalSequence;

            Assert.That(seq._sequence.Count, Is.EqualTo(1));
            Assert.That(seq._sequence[0], Is.InstanceOf<RemoteOperation>());
        }

        [Test]
        public void TestThat_ConditionIsTrue()
        {
            var remSeq = _sequenceManager._sequence[0]._sequence[0] as RemoteSequence;
            var compSeq = remSeq._sequence[0] as CompositeConditionalSequence;

            Assert.That(compSeq._condition(_serverInfo), Is.True);
        }
         
    }


    public class OnlyIfTestApp : ApplicationArtifact, IDependOnInfrastructure<OnlyIfTestInfra>
    {
        public override void Configure(IOfferLocalOperations onLocalMachine, ConDepSettings settings)
        {
            onLocalMachine.ToEachServer(
                server => server
                    .OnlyIf(x => x.OperatingSystem.Name.StartsWith("Windows"))
                    .ExecuteRemote.PowerShell("write-host ''"));
        }
    }

    public class OnlyIfTestInfra : InfrastructureArtifact
    {
        public override void Configure(IOfferInfrastructure require, ConDepSettings settings)
        {
            require
                .OnlyIf(x => x.OperatingSystem.Name.StartsWith("Windows"))
                .RemoteExecution(exec => exec.PowerShell("write-host ''"));
        }
    }

}
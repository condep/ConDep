using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ConDep.Dsl.Builders;
using ConDep.Dsl.Config;
using ConDep.Dsl.Logging;
using ConDep.Dsl.Operations.LoadBalancer;
using ConDep.Dsl.Remote;
using ConDep.Dsl.SemanticModel.Sequence;

namespace ConDep.Dsl.SemanticModel
{
    public class ConDepConfigurationExecutor
    {
        public static void ExecuteFromAssembly(ConDepSettings conDepSettings, IReportStatus status)
        {
            new ConDepConfigurationExecutor().Execute(conDepSettings, status);
        }

        private void Execute(ConDepSettings conDepSettings, IReportStatus status)
        {
            if (conDepSettings == null) { throw new ArgumentException("conDepSettings"); }
            if (conDepSettings.Options.Assembly == null) { throw new ArgumentException("assembly"); }
            if (conDepSettings.Config == null) { throw new ArgumentException("conDepSettings.Config"); }
            if (conDepSettings.Options == null) { throw new ArgumentException("conDepSettings.Options"); }
            if (status == null) { throw new ArgumentException("status"); }

            var applications = CreateApplicationArtifacts(conDepSettings);

            if(!conDepSettings.Options.WebDeployExist)
            {
                var serverValidator = new RemoteServerValidator(conDepSettings.Config.Servers);
                if (!serverValidator.IsValid())
                {
                    Logger.Error("Not all servers fulfill ConDep's requirements. Aborting execution.");
                    return;
                }
            }

            var lbLookup = new LoadBalancerLookup(conDepSettings.Config.LoadBalancer);

            var sequenceManager = new ExecutionSequenceManager(lbLookup.GetLoadBalancer());

            var notification = new Notification();
            var postOpSeq = new PostOpsSequence();

            foreach (var application in applications)
            {
                var infrastructureSequence = new InfrastructureSequence();
                var preOpsSequence = new PreOpsSequence();
                if (!conDepSettings.Options.DeployOnly)
                {
                    var infrastructureBuilder = new InfrastructureBuilder(infrastructureSequence);
                    Configure.InfrastructureOperations = infrastructureBuilder;

                    if (HasInfrastructureDefined(application))
                    {
                        var infrastructureInstance = GetInfrastructureArtifactForApplication(conDepSettings, application);
                        if (!infrastructureSequence.IsValid(notification))
                        {
                            notification.Throw();
                        }
                        infrastructureInstance.Configure(infrastructureBuilder, conDepSettings);
                    }
                }

                var local = new LocalOperationsBuilder(sequenceManager.NewLocalSequence(application.GetType().Name), infrastructureSequence, preOpsSequence, conDepSettings.Config.Servers);
                Configure.LocalOperations = local;

                application.Configure(local, conDepSettings);
            }

            if (!sequenceManager.IsValid(notification))
            {
                notification.Throw();
            }

            try
            {
                sequenceManager.Execute(status, conDepSettings);
            }
            finally
            {
                postOpSeq.Execute(status, conDepSettings);
            }
        }

        private IEnumerable<ApplicationArtifact> CreateApplicationArtifacts(ConDepSettings settings)
        {
            var assembly = settings.Options.Assembly;
            if (settings.Options.HasApplicationDefined())
            {
                var type = assembly.GetTypes().SingleOrDefault(t => typeof (ApplicationArtifact).IsAssignableFrom(t) && t.Name == settings.Options.Application);
                if (type == null)
                {
                    throw new ConDepConfigurationTypeNotFoundException(string.Format("A class inheriting from [{0}] must be present in assembly [{1}] for ConDep to work. No calss with name [{2}] found in assembly. ",typeof (ApplicationArtifact).FullName, assembly.FullName, settings.Options.Application));
                }
                yield return CreateApplicationArtifact(assembly, type);
            }
            else
            {
                var types = assembly.GetTypes().Where(t => typeof(ApplicationArtifact).IsAssignableFrom(t));
                foreach (var type in types)
                {
                    yield return CreateApplicationArtifact(assembly, type);
                }
            }
        }

        private static ApplicationArtifact CreateApplicationArtifact(Assembly assembly, Type type)
        {
            var application = assembly.CreateInstance(type.FullName) as ApplicationArtifact;
            if (application == null) throw new NullReferenceException(string.Format("Instance of application class [{0}] in assembly [{1}] is not found.", type.FullName,assembly.FullName));
            return application;
        }

        private bool HasInfrastructureDefined(ApplicationArtifact application)
        {
            var typeName = typeof(IDependOnInfrastructure<>).Name;
            return application.GetType().GetInterface(typeName) != null;
        }

        private InfrastructureArtifact GetInfrastructureArtifactForApplication(ConDepSettings settings, ApplicationArtifact application)
        {
            var typeName = typeof (IDependOnInfrastructure<>).Name;
            var typeInterface = application.GetType().GetInterface(typeName);
            var infrastructureType = typeInterface.GetGenericArguments().Single();

            var infrastructureInstance = settings.Options.Assembly.CreateInstance(infrastructureType.FullName) as InfrastructureArtifact;
            return infrastructureInstance;
        }
    }
}
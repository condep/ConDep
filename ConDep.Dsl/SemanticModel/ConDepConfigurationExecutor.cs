using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ConDep.Dsl.Builders;
using ConDep.Dsl.Config;
using ConDep.Dsl.Impersonation;
using ConDep.Dsl.Logging;
using ConDep.Dsl.Operations;
using ConDep.Dsl.Operations.LoadBalancer;
using ConDep.Dsl.SemanticModel.Sequence;
using ConDep.Dsl.SemanticModel.WebDeploy;

namespace ConDep.Dsl.SemanticModel
{
    public class ConDepConfigurationExecutor
    {
        public static void ExecuteFromAssembly(Assembly assembly, ConDepConfig envSettings, ConDepOptions options, IReportStatus status)
        {
            new ConDepConfigurationExecutor().Execute(assembly, envSettings, options, status);
        }

        private void Execute(Assembly assembly, ConDepConfig envConfig, ConDepOptions options, IReportStatus status)
        {
            if (assembly == null) { throw new ArgumentException("assembly"); }
            if (envConfig == null) { throw new ArgumentException("envSettings"); }
            if (options == null) { throw new ArgumentException("options"); }
            if (status == null) { throw new ArgumentException("status"); }

            var applications = CreateApplicationArtifacts(options, assembly);

            IoCBootstrapper.Bootstrap(envConfig);

            if(!options.WebDeployExist)
            {
                var serverValidator = new RemoteServerValidator(envConfig.Servers);
                if (!serverValidator.IsValid())
                {
                    Logger.Error("Not all servers fulfill ConDep's requirements. Aborting execution.");
                    return;
                }
            }

            var webDeploy = new WebDeployHandler();
            var lbLookup = new LoadBalancerLookup(envConfig.LoadBalancer);

            var sequenceManager = new ExecutionSequenceManager(lbLookup.GetLoadBalancer());

            var notification = new Notification();
            var postOpSeq = new PostOpsSequence();

            foreach (var application in applications)
            {
                var infrastructureSequence = new InfrastructureSequence();
                var preOpsSequence = new PreOpsSequence();
                if (!options.DeployOnly)
                {
                    var infrastructureBuilder = new InfrastructureBuilder(infrastructureSequence, webDeploy);
                    Configure.InfrastructureOperations = infrastructureBuilder;

                    if (HasInfrastructureDefined(application))
                    {
                        var infrastructureInstance = GetInfrastructureArtifactForApplication(assembly, application);
                        if (!infrastructureSequence.IsValid(notification))
                        {
                            notification.Throw();
                        }
                        infrastructureInstance.Configure(infrastructureBuilder, envConfig);
                    }
                }

                var local = new LocalOperationsBuilder(sequenceManager.NewLocalSequence(application.GetType().Name), infrastructureSequence, preOpsSequence, envConfig.Servers, webDeploy);
                Configure.LocalOperations = local;

                application.Configure(local, envConfig);
            }

            if (!sequenceManager.IsValid(notification))
            {
                notification.Throw();
            }

            sequenceManager.Execute(status, envConfig, options);
            postOpSeq.Execute(status, options);
        }

        private IEnumerable<ApplicationArtifact> CreateApplicationArtifacts(ConDepOptions options, Assembly assembly)
        {
            if (options.HasApplicationDefined())
            {
                var type = assembly.GetTypes().Where(t => typeof (ApplicationArtifact).IsAssignableFrom(t) && t.Name == options.Application).Single();
                if (type == null)
                {
                    throw new ConDepConfigurationTypeNotFoundException(string.Format("A class inheriting from [{0}] must be present in assembly [{1}] for ConDep to work.",typeof (ApplicationArtifact).FullName, assembly.FullName));
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

        private InfrastructureArtifact GetInfrastructureArtifactForApplication(Assembly assembly,
                                                                                      ApplicationArtifact application)
        {
            var typeName = typeof (IDependOnInfrastructure<>).Name;
            var typeInterface = application.GetType().GetInterface(typeName);
            var infrastructureType = typeInterface.GetGenericArguments().Single();

            var infrastructureInstance = assembly.CreateInstance(infrastructureType.FullName) as InfrastructureArtifact;
            return infrastructureInstance;
        }
    }
}
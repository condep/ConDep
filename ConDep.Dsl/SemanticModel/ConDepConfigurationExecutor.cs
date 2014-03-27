using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using ConDep.Dsl.Builders;
using ConDep.Dsl.Config;
using ConDep.Dsl.Logging;
using ConDep.Dsl.Operations;
using ConDep.Dsl.Operations.LoadBalancer;
using ConDep.Dsl.Remote;
using ConDep.Dsl.SemanticModel.Sequence;

namespace ConDep.Dsl.SemanticModel
{
    //Todo: Screaming for refac! 
    public class ConDepConfigurationExecutor
    {
        private bool _cancelled = false;
        private bool _serverNodeInstalled = false;

        public static Task<ConDepExecutionResult> ExecuteFromAssembly(ConDepSettings conDepSettings, IReportStatus status, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();

            return Task.Factory.StartNew(() =>
                {
                    try
                    {
                        if (conDepSettings.Options.Assembly == null)
                        {
                            throw new ArgumentException("assembly");
                        }

                        var clientValidator = new ClientValidator();

                        var serverInfoHarvester = new ServerInfoHarvester(conDepSettings);
                        var serverValidator = new RemoteServerValidator(conDepSettings.Config.Servers,
                                                                        serverInfoHarvester);

                        var lbLookup = new LoadBalancerLookup(conDepSettings.Config.LoadBalancer);
                        var sequenceManager = new ExecutionSequenceManager(lbLookup.GetLoadBalancer());

                        var notification = new Notification();
                        PopulateExecutionSequence(conDepSettings, notification, sequenceManager);

                        if (conDepSettings.Options.DryRun)
                        {
                            sequenceManager.DryRun();
                            return new ConDepExecutionResult(true);
                        }

                        var success = new ConDepConfigurationExecutor().Execute(conDepSettings, clientValidator,
                                                                                serverValidator, sequenceManager, token);
                        return new ConDepExecutionResult(success);
                    }
                    catch (Exception ex)
                    {
                        Logger.Error("An error sneaked by.", ex);
                        throw;
                    }
                }, token);
        }

        public bool Execute(ConDepSettings settings, IValidateClient clientValidator, IValidateServer serverValidator, ExecutionSequenceManager execManager, CancellationToken token)
        {
            if (settings == null) { throw new ArgumentException("settings"); }
            if (settings.Config == null) { throw new ArgumentException("settings.Config"); }
            if (settings.Options == null) { throw new ArgumentException("settings.Options"); }
            if (clientValidator == null) { throw new ArgumentException("clientValidator"); }
            if (serverValidator == null) { throw new ArgumentException("serverValidator"); }
            if (execManager == null) { throw new ArgumentException("execManager"); }

            var status = new StatusReporter();

            Validate(clientValidator, serverValidator);

            ExecutePreOps(settings, status);
            _serverNodeInstalled = true;

            token.Register(() => Cancel(settings, status));
            
            var notification = new Notification();
            if (!execManager.IsValid(notification))
            {
                notification.Throw();
            }

            try
            {
                execManager.Execute(status, settings);
                return true;
            }
            catch (AggregateException aggEx)
            {
                var flattenEx = aggEx.Flatten();
                Logger.Error("ConDep execution failed with the following error(s):");
                foreach (var ex in flattenEx.InnerExceptions)
                {
                    Logger.Error("ConDep execution failed.", ex);
                }
                return false;
            }
            catch (Exception ex)
            {
                Logger.Error("ConDep execution failed.", ex);
                return false;
            }
            finally
            {
                if(!_cancelled) ExecutePostOps(settings, status);
                //new PostOpsSequence().Execute(status, settings);
            }
        }

        private void Cancel(ConDepSettings settings, StatusReporter status)
        {
            Logger.WithLogSection("Cancellation", () =>
                {
                    try
                    {
                        Logger.Warn("Cancelling execution gracefully!");
                        _cancelled = true;
                        if (_serverNodeInstalled) ExecutePostOps(settings, status);
                    }
                    catch (AggregateException aggEx)
                    {
                        foreach (var ex in aggEx.InnerExceptions)
                        {
                            Logger.Error("Failure during cancellation", ex);
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.Error("Failure during cancellation", ex);
                    }
                });
        }

        private static void Validate(IValidateClient clientValidator, IValidateServer serverValidator)
        {
            clientValidator.Validate();

            //var serverInfoHarvester = new ServerInfoHarvester(conDepSettings);
            //var serverValidator = new RemoteServerValidator(conDepSettings.Config.Servers, serverInfoHarvester);
            if (!serverValidator.IsValid())
            {
                throw new ConDepValidationException("Not all servers fulfill ConDep's requirements. Aborting execution.");
            }
        }

        private static void PopulateExecutionSequence(ConDepSettings conDepSettings, Notification notification,
                                               ExecutionSequenceManager sequenceManager)
        {
            var applications = CreateApplicationArtifacts(conDepSettings);
            foreach (var application in applications)
            {
                var infrastructureSequence = new InfrastructureSequence();

                var infrastructureBuilder = new InfrastructureBuilder(infrastructureSequence);
                Configure.InfrastructureOperations = infrastructureBuilder;

                if (HasInfrastructureDefined(application))
                {
                    var infrastructureInstances = GetInfrastructureArtifactForApplication(conDepSettings, application);
                    foreach (var infrastructureInstance in infrastructureInstances)
                    {
                        infrastructureInstance.Configure(infrastructureBuilder, conDepSettings);
                    }

                    if (!infrastructureSequence.IsValid(notification))
                    {
                        notification.Throw();
                    }
                }

                var localSequence = sequenceManager.NewLocalSequence(application.GetType().Name);
                var localBuilder = new LocalOperationsBuilder(localSequence, infrastructureSequence, conDepSettings.Config.Servers);
                Configure.LocalOperations = localBuilder;

                application.Configure(localBuilder, conDepSettings);
            }
        }

        private static void ExecutePreOps(ConDepSettings conDepSettings, IReportStatus status)
        {
            Logger.WithLogSection("Executing pre-operations", () =>
                {
                    foreach (var server in conDepSettings.Config.Servers)
                    {
                        Logger.WithLogSection(server.Name, () =>
                            {
                                //Todo: This will not work with ConDep server. After first run, this key will always exist.
                                if (!ConDepGlobals.ServersWithPreOps.ContainsKey(server.Name))
                                {
                                    var remotePreOps = new PreRemoteOps();
                                    remotePreOps.Execute(server, status, conDepSettings);
                                    ConDepGlobals.ServersWithPreOps.Add(server.Name, server);
                                }
                            });
                    }
                });
        }

        private static void ExecutePostOps(ConDepSettings conDepSettings, IReportStatus status)
        {
            foreach (var server in conDepSettings.Config.Servers)
            {
                //Todo: This will not work with ConDep server. After first run, this key will always exist.
                if (ConDepGlobals.ServersWithPreOps.ContainsKey(server.Name))
                {
                    var remotePostOps = new PostRemoteOps();
                    remotePostOps.Execute(server, status, conDepSettings);
                    ConDepGlobals.ServersWithPreOps.Remove(server.Name);
                }
            }
        }

        private static IEnumerable<ApplicationArtifact> CreateApplicationArtifacts(ConDepSettings settings)
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

        private static bool HasInfrastructureDefined(ApplicationArtifact application)
        {
            var typeName = typeof(IDependOnInfrastructure<>).Name;
            return application.GetType().GetInterface(typeName) != null;
        }

        private static IEnumerable<InfrastructureArtifact> GetInfrastructureArtifactForApplication(ConDepSettings settings, ApplicationArtifact application)
        {
            var typeName = typeof (IDependOnInfrastructure<>).Name;
            var typeInterface = application.GetType().GetInterface(typeName);
            var infrastructureTypes = typeInterface.GetGenericArguments();

            return infrastructureTypes.Select(type => settings.Options.Assembly.CreateInstance(type.FullName) as InfrastructureArtifact);
        }
    }
}
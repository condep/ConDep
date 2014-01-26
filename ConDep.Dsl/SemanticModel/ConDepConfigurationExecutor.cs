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

        public static ConDepExecutionResult ExecuteFromAssembly(ConDepSettings conDepSettings, CancellationToken token)
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

                return new ConDepConfigurationExecutor().Execute(conDepSettings, clientValidator,
                                                                        serverValidator, sequenceManager, token);
            }
            catch (Exception ex)
            {
                Logger.Error("An error sneaked by.", ex);
                throw;
            }
        }

        public static Task<ConDepExecutionResult> ExecuteFromAssemblyAsync(ConDepSettings conDepSettings, CancellationToken token)
        {
            return Task.Factory.StartNew(() => ExecuteFromAssembly(conDepSettings, token), token);
        }

        public ConDepExecutionResult Execute(ConDepSettings settings, IValidateClient clientValidator, IValidateServer serverValidator, ExecutionSequenceManager execManager, CancellationToken token)
        {
            if (settings == null) { throw new ArgumentException("settings"); }
            if (settings.Config == null) { throw new ArgumentException("settings.Config"); }
            if (settings.Options == null) { throw new ArgumentException("settings.Options"); }
            if (clientValidator == null) { throw new ArgumentException("clientValidator"); }
            if (serverValidator == null) { throw new ArgumentException("serverValidator"); }
            if (execManager == null) { throw new ArgumentException("execManager"); }

            var status = new StatusReporter();

            try
            {
                Validate(clientValidator, serverValidator);

                ExecutePreOps(settings, status, token);
                _serverNodeInstalled = true;

            token.Register(() => Cancel(settings, status));
            
            var notification = new Notification();
            if (!execManager.IsValid(notification))
            {
                notification.Throw();
            }

                execManager.Execute(status, settings, token);
                return new ConDepExecutionResult(true);
            }
            catch (OperationCanceledException)
            {
                Cancel(settings, status, token);
                return new ConDepExecutionResult(false) { Cancelled = true };
            }
            catch (AggregateException aggEx)
            {
                var result = new ConDepExecutionResult(false);
                var flattenEx = aggEx.Flatten();
                    {
                foreach (var ex in flattenEx.InnerExceptions)
                        {
                            Cancel(settings, status, token);
                            result.Cancelled = true;
                            Logger.Warn("ConDep execution cancelled.");
                        }
                        else
                        {
                            result.AddException(inner);
                            Logger.Error("ConDep execution failed.", inner);
                        }
                        
                        return true;
                    });
                return result;
            }
            catch (Exception ex)
            {
                var result = new ConDepExecutionResult(false);
                result.AddException(ex);
                Logger.Error("ConDep execution failed.", ex);
                return result;
            }
            finally
            {
                if(!_cancelled) ExecutePostOps(settings, status, token);
                //new PostOpsSequence().Execute(status, settings);
            }
        }

        private void Cancel(ConDepSettings settings, StatusReporter status, CancellationToken token)
        {
            Logger.WithLogSection("Cancellation", () =>
                {
                    try
                    {
                        var tokenSource = new CancellationTokenSource();
                        Logger.Warn("Cancelling execution gracefully!");
                        _cancelled = true;
                        if (_serverNodeInstalled) ExecutePostOps(settings, status, tokenSource.Token);
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

        private static void ExecutePreOps(ConDepSettings conDepSettings, IReportStatus status, CancellationToken token)
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
                                    remotePreOps.Execute(server, status, conDepSettings, token);
                                    ConDepGlobals.ServersWithPreOps.Add(server.Name, server);
                                }
                            });
                    }
                });
        }

        private static void ExecutePostOps(ConDepSettings conDepSettings, IReportStatus status, CancellationToken token)
        {
            foreach (var server in conDepSettings.Config.Servers)
            {
                //Todo: This will not work with ConDep server. After first run, this key will always exist.
                if (ConDepGlobals.ServersWithPreOps.ContainsKey(server.Name))
                {
                    var remotePostOps = new PostRemoteOps();
                    remotePostOps.Execute(server, status, conDepSettings, token);
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
            var interfaces = application.GetType().GetInterfaces();
            return interfaces.Any(x => x.Name == typeName);
        }

        private static IEnumerable<InfrastructureArtifact> GetInfrastructureArtifactForApplication(ConDepSettings settings, ApplicationArtifact application)
        {
            var typeName = typeof (IDependOnInfrastructure<>).Name;
            var typeInterfaces = application.GetType().GetInterfaces();

            var infraInterfaces = typeInterfaces.Where(x => x.Name == typeName);
            foreach (var infraInterface in infraInterfaces)
            {
                var infrastructureType = infraInterface.GetGenericArguments().Single();

                var infrastructureInstance = settings.Options.Assembly.CreateInstance(infrastructureType.FullName) as InfrastructureArtifact;
                yield return infrastructureInstance;
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ConDep.Dsl.Experimental.Application;
using ConDep.Dsl.Experimental.Application.Deployment;
using ConDep.Dsl.Experimental.Core;
using ConDep.Dsl.Model.Config;
using ConDep.Dsl.WebDeploy;
using TinyIoC;

namespace ConDep.Dsl
{
    public class ConDepConfigurationExecutor
    {
        public void Execute(Assembly assembly, ConDepConfig envConfig, ConDepOptions options, IReportStatus status)
        {
            if (assembly == null) { throw new ArgumentException("assembly"); }
            if (envConfig == null) { throw new ArgumentException("envSettings"); }

            var type = assembly.GetTypes().Where(t => typeof(ConDepDefinitionBase).IsAssignableFrom(t)).FirstOrDefault();
            if (type == null)
            {
                throw new ConDepConfigurationTypeNotFoundException(string.Format("A class inheriting from [{0}] must be present in assembly [{1}] for ConDep to work.", typeof(ConDepDefinitionBase).FullName, assembly.FullName));
            }

            var depObject = assembly.CreateInstance(type.FullName) as ConDepDefinitionBase;
            if (depObject == null) throw new NullReferenceException(string.Format("Instance of configuration class [{0}] in assembly [{1}] is null.", type.FullName, assembly.FullName));

            depObject.Options = options;
            depObject.Status = status;
            depObject.EnvSettings = envConfig;

            IoCBootstrapper.Bootstrap(envConfig);

            var conDepSetup = TinyIoCContainer.Current.Resolve<ISetupConDep>();
            var notification = new Notification();

            depObject.Configure((IProvideForSetup)conDepSetup);

            if (!conDepSetup.IsValid(notification))
            {
                notification.Throw();
            }

            conDepSetup.Execute(options, status);
        }

        public static void ExecuteFromAssembly(Assembly assembly, ConDepConfig envSettings, ConDepOptions options, IReportStatus status)
        {
            new ConDepConfigurationExecutor().Execute(assembly, envSettings, options, status);
        }

        public static void ExecuteExperimentalFromAssembly(Assembly assembly, ConDepConfig envSettings, ConDepOptions options, IReportStatus status)
        {
            new ConDepConfigurationExecutor().ExecuteExperimental(assembly, envSettings, options, status);
        }

        private void ExecuteExperimental(Assembly assembly, ConDepConfig envConfig, ConDepOptions options, IReportStatus status)
        {
            if (assembly == null) { throw new ArgumentException("assembly"); }
            if (envConfig == null) { throw new ArgumentException("envSettings"); }
            if (options == null) { throw new ArgumentException("options"); }
            if (status == null) { throw new ArgumentException("status"); }

            var applications = new List<ApplicationArtifact>();
            if(options.HasContext())
            {
                var type = assembly.GetTypes().Where(t => typeof(ApplicationArtifact).IsAssignableFrom(t) && t.Name == options.Context).Single();
                if (type == null)
                {
                    throw new ConDepConfigurationTypeNotFoundException(string.Format("A class inheriting from [{0}] must be present in assembly [{1}] for ConDep to work.", typeof(ConDepDefinitionBase).FullName, assembly.FullName));
                }

                var application = assembly.CreateInstance(type.FullName) as ApplicationArtifact;
                if (application == null) throw new NullReferenceException(string.Format("Instance of application class [{0}] in assembly [{1}] is not found.", type.FullName, assembly.FullName));
                applications.Add(application);
            } 
            else
            {
                var types = assembly.GetTypes().Where(t => typeof(ApplicationArtifact).IsAssignableFrom(t));
                foreach(var type in types)
                {
                    var application = assembly.CreateInstance(type.FullName) as ApplicationArtifact;
                    if (application == null) throw new NullReferenceException(string.Format("Instance of application class [{0}] in assembly [{1}] is not found.", type.FullName, assembly.FullName));
                    applications.Add(application);
                }
            }

            IoCBootstrapper.Bootstrap(envConfig);
            var sequence = TinyIoCContainer.Current.Resolve<IManageExecutionSequence>();

            foreach(var application in applications)
            {
                var local = new ApplicationOps(sequence);
                application.Configure(local);
            }

            var notification = new Notification();
            if (!sequence.IsValid(notification))
            {
                notification.Throw();
            }

            sequence.Execute(status);
        }
    }
}
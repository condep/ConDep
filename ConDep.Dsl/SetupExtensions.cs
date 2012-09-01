using System;
using TinyIoC;

namespace ConDep.Dsl
{
	public static class SetupExtensions
	{
        /// <summary>
        /// A collection of deployment specific providers for moving content from A to B.
        /// </summary>
        /// <param name="conDepSetup"></param>
        /// <param name="deployment"></param>
        public static void Deployment(this IProvideForSetup conDepSetup, Action<ProvideForDeployment> deployment)
        {
            var setup = (ConDepSetup)conDepSetup;
            foreach (var deploymentServer in setup.EnvSettings.Servers)
            {
                setup.WebDeploySetup.ConfigureServer(deploymentServer);

                var webDeployOperation = new WebDeployOperation(setup.WebDeploySetup.ActiveWebDeployServerDefinition);
                setup.AddOperation(webDeployOperation);

                var provideForDeployment = new ProvideForDeployment();
                ((IProvideOptions)provideForDeployment).WebDeploySetup = setup.WebDeploySetup;
                ((IProvideOptions)provideForDeployment).AddProviderAction = setup.WebDeploySetup.ConfigureProvider;
                deployment(provideForDeployment);
            }
        }

        /// <summary>
        /// A collection of infrastructure specific providers for doing everything from configuring Windows, IIS, run commands etc.
        /// </summary>
        /// <param name="conDepSetup"></param>
        /// <param name="infrastructure"></param>
        public static void Infrastructure(this IProvideForSetup conDepSetup, Action<ProvideForInfrastructure> infrastructure)
        {
            var setup = (ConDepSetup)conDepSetup;
            foreach (var deploymentServer in setup.EnvSettings.Servers)
            {
                //Should WebDeploySetup (or its functionality) be on the WebDeployOperation?
                setup.WebDeploySetup.ConfigureServer(deploymentServer);

                var webDeployOperation = new WebDeployOperation(setup.WebDeploySetup.ActiveWebDeployServerDefinition);
                setup.AddOperation(webDeployOperation);

                var provideInfra = new ProvideForInfrastructure();
                ((IProvideOptions)provideInfra).WebDeploySetup = setup.WebDeploySetup;
                ((IProvideOptions)provideInfra).AddProviderAction = setup.WebDeploySetup.ConfigureProvider;
                infrastructure(provideInfra);
            }
        }

        /// <summary>
        /// Use this to create a context for your configuration to execute in. 
        /// Typically you use different contexts to seperate deployment/infrastructure-logic 
        /// needed for an application, web site or similar from others.
        /// When executed from the command line, you can optionally sepcify which context you 
        /// want to execute. If no context is specified, everything will be executed.
        /// </summary>
        /// <param name="contextName">Name of the application context to use. This is the value used from command line to define which context to execute.</param>
        public static void ConDepContext(this IProvideForSetup setup, string contextName, Action<IProvideForSetup> contextSetup)
        {
            var parentSetup = setup as ConDepSetup;
            var conDepSetup = TinyIoCContainer.Current.Resolve<ISetupConDep>();

            parentSetup.Context.Add((ISetupConDep)conDepSetup, contextName);
            parentSetup.AddOperation(new ConDepContextOperationPlaceHolder(contextName));
            contextSetup((IProvideForSetup)conDepSetup);
        }
    }
}

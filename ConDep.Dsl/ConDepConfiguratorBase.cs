using System;
using System.Diagnostics;
using System.IO;
using ConDep.Dsl.Model.Config;
using ConDep.Dsl.WebDeploy;
using TinyIoC;

namespace ConDep.Dsl
{
	public abstract class ConDepConfiguratorBase
	{
	    protected internal void Setup(Action<IProvideForSetup> action)
		{
	        var conDepSetup = TinyIoCContainer.Current.Resolve<ISetupConDep>();
            var notification = new Notification();

            action((IProvideForSetup)conDepSetup);

			if (!conDepSetup.IsValid(notification))
			{
				notification.Throw();
			}

            conDepSetup.Execute(Options, Status);
		}

        protected internal ConDepOptions Options { get; set; }

	    public WebDeploymentStatus Status { get; set; }

        protected internal ConDepConfig EnvSettings { get; set; }

	    protected internal abstract void Configure();
	}
}
using System;
using System.Diagnostics;
using System.IO;
using ConDep.Dsl.Model.Config;
using ConDep.Dsl.WebDeploy;
using TinyIoC;

namespace ConDep.Dsl
{
	public abstract class ConDepDefinitionBase
	{
        protected internal ConDepOptions Options { get; set; }

	    public WebDeploymentStatus Status { get; set; }

        protected internal ConDepConfig EnvSettings { get; set; }

        protected internal abstract void Configure(IProvideForSetup setup);
	}
}
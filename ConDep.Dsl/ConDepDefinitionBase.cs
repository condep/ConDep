using ConDep.Dsl.Experimental.Core;
using ConDep.Dsl.Model.Config;

namespace ConDep.Dsl
{
	public abstract class ConDepDefinitionBase
	{
        protected internal ConDepOptions Options { get; set; }

	    public IReportStatus Status { get; set; }

        protected internal ConDepConfig EnvSettings { get; set; }

        protected internal abstract void Configure(IProvideForSetup setup);
	}
}
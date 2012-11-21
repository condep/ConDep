using System;

namespace ConDep.Dsl.Experimental.Application
{
    public abstract class ApplicationArtifact
    {
        public abstract void Configure(IOfferApplicationOps local);
    }
}
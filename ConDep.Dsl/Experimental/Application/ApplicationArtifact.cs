using System;
using ConDep.Dsl.Model.Config;

namespace ConDep.Dsl.Experimental.Application
{
    public abstract class ApplicationArtifact
    {
        public abstract void Configure(IOfferApplicationOps local, ConDepConfig config);
    }
}
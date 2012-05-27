using System;
using ConDep.Dsl.Operations.WebDeploy.Options;

namespace ConDep.Dsl.Builders
{
    public class IisOptions
    {
        public void FromExistingServer(string sourceServer, Action<IProvideForExistingIisServer> action)
        {
            throw new NotImplementedException();
        }

        public void FromCustomDefinition(Action<IProvideForCustomIis> action)
        {
            throw new NotImplementedException();
        }
    }
}
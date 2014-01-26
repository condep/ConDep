using System;
using System.Linq;
using System.Reflection;
using ConDep.Dsl;

namespace ConDep.Server.Execution
{
    public class ArtifactResolver : MarshalByRefObject
    {
        public string[] Resolve(string assemblyFilePath)
        {
            var assembly = Assembly.LoadFile(assemblyFilePath);
            var types = assembly.GetTypes().Where(t => typeof(ApplicationArtifact).IsAssignableFrom(t));
            return types.Select(type => type.Name.Replace("Artifact", "")).ToArray();
        }
    }
}
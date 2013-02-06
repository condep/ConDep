using System.Collections.Generic;
using ConDep.Dsl.Resources;

namespace ConDep.Dsl.Builders
{
    public class ConDepPowerShellScripts
    {
        public IEnumerable<string> ScriptPaths
        {
            get
            {
                return null;
                //ConDepResourceFiles.GetFilePath(typeof (ScriptNamespaceMarker).Namespace);
            }
        }
    }
}
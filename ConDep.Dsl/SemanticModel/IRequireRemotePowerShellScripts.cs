using System.Collections.Generic;

namespace ConDep.Dsl.SemanticModel
{
    public interface IRequireRemotePowerShellScripts
    {
        IEnumerable<string> ScriptPaths { get; }
    }
}
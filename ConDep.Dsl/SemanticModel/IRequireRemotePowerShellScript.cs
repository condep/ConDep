using System.Collections.Generic;

namespace ConDep.Dsl.SemanticModel
{
    public interface IRequireRemotePowerShellScript
    {
        IEnumerable<string> ScriptPaths { get; }
    }
}
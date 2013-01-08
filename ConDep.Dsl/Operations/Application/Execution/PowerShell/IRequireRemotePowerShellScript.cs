using System.Collections.Generic;

namespace ConDep.Dsl.Operations.Application.Execution.PowerShell
{
    public interface IRequireRemotePowerShellScript
    {
        IEnumerable<string> ScriptPaths { get; }
    }
}
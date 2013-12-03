using System.Threading;

namespace ConDep.Server.Execution
{
    public interface ITokenSource
    {
        CancellationToken Token { get; }
    }
}
using System;
using ConDep.Dsl.Config;

namespace ConDep.Dsl
{
    public interface IOfferRemoteOperations
    {
        /// <summary>
        /// Provide operations for remote deployment.
        /// </summary>
        IOfferRemoteDeployment Deploy { get; }

        /// <summary>
        /// Provide operations for remote execution.
        /// </summary>
        IOfferRemoteExecution ExecuteRemote { get; }

        /// <summary>
        /// Server side condition. Any Operation followed by <see cref="OnlyIf"/> will only execute if the condition is met.
        /// </summary>
        /// <param name="condition">The condition that must be met</param>
        /// <returns></returns>
        IOfferRemoteComposition OnlyIf(Predicate<ServerInfo> condition);
    }
}
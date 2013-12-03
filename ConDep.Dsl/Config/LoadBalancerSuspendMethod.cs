using System;

namespace ConDep.Dsl.Config
{
    [Serializable]
    public enum LoadBalancerSuspendMethod
    {
        Graceful,
        Suspend,
        SuspendClearConnections
    }
}
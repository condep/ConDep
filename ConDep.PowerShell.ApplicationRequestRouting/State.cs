namespace ConDep.PowerShell.ApplicationRequestRouting
{
    public enum State
    {
        Offline,
        Online,
        Unavailable,
        Available,
        DisallowNewConnections,
        Healthy,
        Unhealthy
    }
}
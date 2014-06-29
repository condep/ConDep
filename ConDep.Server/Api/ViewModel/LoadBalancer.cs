using System;
using System.Collections.Generic;

namespace ConDep.Server.Api.ViewModel
{
    public class LoadBalancer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Provider { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public SuspendStrategy SuspendStrategy { get; set; }
        public SchedulingAlgorithm SchedulingAlgorithm { get; set; }
        public List<Farm> Farms { get; set; } 
    }

    public class Farm
    {
        public string Name { get; set; }
    }

    public enum SuspendStrategy
    {
        Graceful,
        Suspend,
        SuspendClearConnections
    }

    public enum SchedulingAlgorithm
    {
        RoundRobin,
        Sticky
    }
}
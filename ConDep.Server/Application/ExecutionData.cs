using System;

namespace ConDep.Server.Application
{
    public class ExecutionData
    {
        public string Environment { get; set; }

        public Guid DeploymentId { get; set; }

        public string Artifact { get; set; }

        public string Module { get; set; }
    }
}
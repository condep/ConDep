using System;

namespace ConDep.Server.Api.Model
{
    public class ExecutionStatus
    {
        public string ExecId { get; set; }
        public string Status { get; set; }
        public DateTime UpdatedUtc { get; set; }
    }
}
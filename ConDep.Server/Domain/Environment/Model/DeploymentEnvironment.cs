using System;
using System.Collections.Generic;
using ConDep.Server.Domain.Infrastructure;

namespace ConDep.Server.Domain.Environment.Model
{
    public class DeploymentEnvironment : IAggregateRoot
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public LoadBalancer LoadBalancer { get; private set; }
        public IEnumerable<Tier> Tiers { get; private set; }
        public DeploymentUser DeploymentUser { get; private set; }
    }

    public class DeploymentUser
    {
        public string UserName { get; private set; }
        public string Password { get; private set; }
    }

    public class Tier
    {
        public IEnumerable<Server> Servers { get; private set; }
 
    }

    public class LoadBalancer
    {
    }

    public class Server
    {
        
    }
}
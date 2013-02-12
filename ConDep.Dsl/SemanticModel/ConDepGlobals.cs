using System;
using System.Collections.Generic;
using ConDep.Dsl.Config;

namespace ConDep.Dsl.SemanticModel
{
    public static class ConDepGlobals
    {
        public static readonly Guid ExecId = Guid.NewGuid();
        public static readonly Dictionary<string, ServerConfig> ServersWithPreOps = new Dictionary<string, ServerConfig>();
    }
}
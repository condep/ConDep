using System;
using System.Collections.Generic;
using ConDep.Dsl.Config;

namespace ConDep.Dsl.SemanticModel
{
    public static class ConDepGlobals
    {
        public static Guid ExecId = Guid.NewGuid();
        public static readonly Dictionary<string, ServerConfig> ServersWithPreOps = new Dictionary<string, ServerConfig>();

        public static void Reset()
        {
            ExecId = Guid.NewGuid();
            ServersWithPreOps.Clear();
        }
    }
}
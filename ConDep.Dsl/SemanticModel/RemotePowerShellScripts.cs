using System.Collections.Generic;

namespace ConDep.Dsl.SemanticModel
{
    public static class RemotePowerShellScripts
    {
        private static readonly List<string> _scripts = new List<string>();

        public static void Add(IEnumerable<string> scriptPaths)
        {
            _scripts.AddRange(scriptPaths);
        }

        public static void Add(string scriptPath)
        {
            _scripts.Add(scriptPath);
        }

        internal static IEnumerable<string> Scripts
        {
            get { return _scripts; }
        } 
    }
}
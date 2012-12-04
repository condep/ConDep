using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ConDep.Dsl.SemanticModel
{
    internal class ConDepContext : IEnumerable<ApplicationArtifact>
    {
        private readonly List<ApplicationArtifact> _context = new List<ApplicationArtifact>();

        public static string Default
        {
            get { return "Default"; }
        }

        public void Add(ApplicationArtifact app)
        {
            _context.Add(app);    
        }

        public ApplicationArtifact this[string appName]
        {
            get { return _context.Single(x => x.GetType().Name.ToLower() == appName.ToLower()); }
        }

        public bool HasContext()
        {
            return _context.Count > 0;
        }

        public IEnumerator<ApplicationArtifact> GetEnumerator()
        {
            return _context.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
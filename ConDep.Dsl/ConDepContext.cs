using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ConDep.Dsl
{
    public class ConDepContext : IEnumerable<ISetupConDep>
    {
        private readonly Dictionary<string, ISetupConDep> _context = new Dictionary<string, ISetupConDep>();

        public static string Default
        {
            get { return "Default"; }
        }

        public void Add(ISetupConDep conDepSetup, string appName)
        {
            _context.Add(appName, conDepSetup);    
        }

        public ISetupConDep this[string appName]
        {
            get { return _context[appName]; }
        }

        public ISetupConDep this[int index]
        {
            get { return _context.ToList()[index].Value; }
        }

        public bool HasContext()
        {
            return _context.Count > 0;
        }

        public IEnumerator<ISetupConDep> GetEnumerator()
        {
            return _context.Values.ToList().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
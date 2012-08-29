using System.Collections.Generic;

namespace ConDep.Dsl.Core
{
    public class ConDepAppContext
    {
        private Dictionary<string, ISetupConDep> _context = new Dictionary<string, ISetupConDep>();
 
        public void Add(ISetupConDep conDepSetup, string appName)
        {
            _context.Add(appName, conDepSetup);    
        }

        public ISetupConDep this[string appName]
        {
            get { return _context[appName]; }
        }

        public bool HasContext()
        {
            return _context.Count > 0;
        }
    }
}
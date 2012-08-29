using System.Diagnostics;

namespace ConDep.Dsl
{
    public class ConDepOptions
    {
        private readonly string _context;
        private readonly bool _deployOnly;
        private readonly bool _infraOnly;
        private readonly TraceLevel _traceLevel;

        public ConDepOptions(string context, bool deployOnly, bool infraOnly, TraceLevel traceLevel)
        {
            _context = context;
            _deployOnly = deployOnly;
            _infraOnly = infraOnly;
            _traceLevel = traceLevel;
        }

        public string Context
        {
            get { return _context; }
        }

        public bool DeployOnly
        {
            get { return _deployOnly; }
        }

        public bool InfraOnly
        {
            get { return _infraOnly; }
        }

        public TraceLevel TraceLevel
        {
            get { return _traceLevel; }
        }

        public bool HasContext()
        {
            return !string.IsNullOrWhiteSpace(Context);
        }
    }
}
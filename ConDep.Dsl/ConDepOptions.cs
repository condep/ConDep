using System.Diagnostics;

namespace ConDep.Dsl
{
    public class ConDepOptions
    {
        private readonly string _context;
        private readonly bool _deployOnly;
        private readonly bool _infraOnly;
        private readonly TraceLevel _traceLevel;
        private readonly bool _printSequence;

        public ConDepOptions(string context, bool deployOnly, bool infraOnly, TraceLevel traceLevel, bool printSequence)
        {
            _context = context;
            _deployOnly = deployOnly;
            _infraOnly = infraOnly;
            _traceLevel = traceLevel;
            _printSequence = printSequence;
        }

        public string Context { get { return string.IsNullOrWhiteSpace(_context) ? ConDepContext.Default : _context; } }

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

        public bool PrintSequence
        {
            get { return _printSequence; }
        }

        public bool HasContext()
        {
            return !string.IsNullOrWhiteSpace(Context) && Context != ConDepContext.Default;
        }
    }
}
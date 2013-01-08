namespace ConDep.Dsl.SemanticModel
{
    public class ConDepOptions
    {
        private readonly string _context;
        private readonly bool _deployOnly;
        private readonly bool _infraOnly;
        private readonly bool _webDeployExist;

        public ConDepOptions(string context, bool deployOnly, bool infraOnly, bool webDeployExist)
        {
            _context = context;
            _deployOnly = deployOnly;
            _infraOnly = infraOnly;
            _webDeployExist = webDeployExist;
        }

        public string Context { get { return string.IsNullOrWhiteSpace(_context) ? "Default" : _context; } }

        public bool DeployOnly
        {
            get { return _deployOnly; }
        }

        public bool InfraOnly
        {
            get { return _infraOnly; }
        }

        public bool WebDeployExist
        {
            get { return _webDeployExist; }
        }

        public bool HasContext()
        {
            return !string.IsNullOrWhiteSpace(Context) && Context != "Default";
        }
    }
}
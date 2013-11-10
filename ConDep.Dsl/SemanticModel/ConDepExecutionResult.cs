namespace ConDep.Dsl.SemanticModel
{
    public class ConDepExecutionResult
    {
        private readonly bool _success;

        public ConDepExecutionResult(bool success)
        {
            _success = success;
        }

        public bool Success
        {
            get { return _success; }
        }
    }
}
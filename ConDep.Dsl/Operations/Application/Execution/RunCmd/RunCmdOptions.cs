namespace ConDep.Dsl.Operations.Application.Execution.RunCmd
{
    public class RunCmdOptions : IOfferRunCmdOptions
    {
        private readonly RunCmdOptionValues _values = new RunCmdOptionValues();

        public IOfferRunCmdOptions ContinueOnError(bool continueOnError)
        {
            Values.ContinueOnError = continueOnError;
            return this;
        }

        public class RunCmdOptionValues
        {
            public bool ContinueOnError { get; set; }
        }

        public RunCmdOptionValues Values { get { return _values; } }

    }

}
namespace ConDep.Dsl.Operations.Application.Execution.PowerShell
{
    public class PowerShellOptions : IOfferPowerShellOptions
    {
        private readonly PowerShellOptionValues _values = new PowerShellOptionValues();

        public IOfferPowerShellOptions RequireRemoteLib()
        {
            _values.RequireRemoteLib = true;
            return this;
        }

        public IOfferPowerShellOptions ContinueOnError(bool value)
        {
            _values.ContinueOnError = value;
            return this;
        }

        public PowerShellOptionValues Values { get { return _values; } }

        public class PowerShellOptionValues
        {
            public bool RequireRemoteLib { get; set; }

            public bool ContinueOnError { get; set; }
        }
    }

}
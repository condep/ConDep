namespace ConDep.Dsl.Operations.Application.Deployment.WindowsService
{
    public class WindowsServiceFailureOptions : IOfferWindowsServiceFailureOptions
    {
        private readonly WindowsServiceFailureOptionValues _values = new WindowsServiceFailureOptionValues();

        public IOfferWindowsServiceFailureActions FirstFailure { get { return new WindowsServiceFailureActions(_values.FirstFailure, this); } }
        public IOfferWindowsServiceFailureActions SecondFailure { get { return new WindowsServiceFailureActions(_values.SecondFailure, this); } }
        public IOfferWindowsServiceFailureActions SubsequentFailures { get { return new WindowsServiceFailureActions(_values.SubsequentFailures, this); } }
        public WindowsServiceFailureOptionValues Values { get { return _values; } }

        public class WindowsServiceFailureOptionValues
        {
            private readonly WindowsServiceFailureActions.WindowsServiceFailureActionValues _firstFailure = new WindowsServiceFailureActions.WindowsServiceFailureActionValues();
            private readonly WindowsServiceFailureActions.WindowsServiceFailureActionValues _secondFailure = new WindowsServiceFailureActions.WindowsServiceFailureActionValues();
            private readonly WindowsServiceFailureActions.WindowsServiceFailureActionValues _subsequentFailures = new WindowsServiceFailureActions.WindowsServiceFailureActionValues();

            public WindowsServiceFailureActions.WindowsServiceFailureActionValues FirstFailure { get { return _firstFailure; } }
            public WindowsServiceFailureActions.WindowsServiceFailureActionValues SecondFailure { get { return _secondFailure; } }
            public WindowsServiceFailureActions.WindowsServiceFailureActionValues SubsequentFailures { get { return _subsequentFailures; } }
        }
    }

    public class WindowsServiceFailureActions : IOfferWindowsServiceFailureActions
    {
        private WindowsServiceFailureActionValues _failure;
        private readonly IOfferWindowsServiceFailureOptions _caller;

        public WindowsServiceFailureActions(WindowsServiceFailureActionValues failure, IOfferWindowsServiceFailureOptions caller)
        {
            _failure = failure;
            _caller = caller;
        }

        public IOfferWindowsServiceFailureOptions TakeNoAction()
        {
            _failure.Action = "none/100";
            return _caller;
        }

        public IOfferWindowsServiceFailureOptions RestartService(int delayInMilliseconds)
        {
            _failure.Action = "restart/" + delayInMilliseconds;
            return _caller;
        }

        public IOfferWindowsServiceFailureOptions RunProgram(string program, string parameters, int delayInMilliseconds)
        {
            _failure.Action = "run/" + delayInMilliseconds;
            _failure.Command = string.Format("\"{0} {1}\"", program, parameters);
            return _caller;
        }

        public IOfferWindowsServiceFailureOptions RestartComputer(int delayInMilliseconds)
        {
            _failure.Action = "reboot/" + delayInMilliseconds;
            return _caller;
        }

        public class WindowsServiceFailureActionValues
        {
            private string _action = "none/100";
            public string Action { get { return _action; } set { _action = value; } }
            public string Command { get; set; }
        }
    }
}
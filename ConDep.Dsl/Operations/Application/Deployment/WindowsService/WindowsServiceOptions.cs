namespace ConDep.Dsl.Builders
{
    public class WindowsServiceOptions : IOfferWindowsServiceOptions
    {
        private readonly WindowsServiceOptionValues _values = new WindowsServiceOptionValues();
        
        public WindowsServiceOptions UseServiceInstaller(string parameters)
        {
            _values.ServiceInstallerParams = parameters;
            return this;
        }

        public WindowsServiceOptions UserName(string username)
        {
            _values.UserName = username;
            return this;
        }

        public WindowsServiceOptions Password(string password)
        {
            _values.Password = password;
            return this;
        }

        public WindowsServiceOptions DisplayName(string displayName)
        {
            _values.DisplayName = displayName;
            return this;
        }

        public WindowsServiceOptions Description(string description)
        {
            _values.Description = description;
            return this;
        }

        public WindowsServiceOptions ServiceGroup(string group)
        {
            _values.ServiceGroup = group;
            return this;
        }

        public WindowsServiceOptions ExeParams(string parameters)
        {
            _values.ExeParams = parameters;
            return this;
        }

        public WindowsServiceOptions ServiceFailureResetInterval(int interval)
        {
            _values.ServiceFailureResetInterval = interval;
            return this;
        }

        public WindowsServiceOptions ServiceRestartDelay(int delay)
        {
            _values.ServiceRestartDelay = delay;
            return this;
        }

        public WindowsServiceOptions IgnoreFailureOnServiceStartStop(bool value)
        {
            _values.IgnoreFailureOnServiceStartStop = value;
            return this;
        }

        public WindowsServiceOptionValues Values { get { return _values; } }

        public class WindowsServiceOptionValues
        {
            public string UserName { get; set; }
            public string Password { get; set; }
            public string DisplayName { get; set; }
            public string Description { get; set; }
            public string ServiceGroup { get; set; }
            public string ExeParams { get; set; }
            public int ServiceFailureResetInterval { get; set; }
            public int ServiceRestartDelay { get; set; }
            public bool IgnoreFailureOnServiceStartStop { get; set; }
            public string ServiceInstallerParams { get; set; }
        }
    }
}
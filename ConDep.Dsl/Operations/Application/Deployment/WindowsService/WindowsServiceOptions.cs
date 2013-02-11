using System;
using System.ServiceProcess;

namespace ConDep.Dsl.Operations.Application.Deployment.WindowsService
{
    public class WindowsServiceOptions : IOfferWindowsServiceOptions
    {
        private readonly WindowsServiceOptionValues _values = new WindowsServiceOptionValues();

        public IOfferWindowsServiceOptions UserName(string username)
        {
            _values.UserName = username;
            return this;
        }

        public IOfferWindowsServiceOptions Password(string password)
        {
            _values.Password = password;
            return this;
        }

        //public WindowsServiceOptions DisplayName(string displayName)
        //{
        //    _values.DisplayName = displayName;
        //    return this;
        //}

        public IOfferWindowsServiceOptions Description(string description)
        {
            _values.Description = description;
            return this;
        }

        public IOfferWindowsServiceOptions ServiceGroup(string group)
        {
            _values.ServiceGroup = group;
            return this;
        }

        public IOfferWindowsServiceOptions ExeParams(string parameters)
        {
            _values.ExeParams = parameters;
            return this;
        }

        public IOfferWindowsServiceOptions ServiceFailureResetInterval(int interval)
        {
            _values.ServiceFailureResetInterval = interval;
            return this;
        }

        public IOfferWindowsServiceOptions ServiceRestartDelay(int delay)
        {
            _values.ServiceRestartDelay = delay;
            return this;
        }

        public IOfferWindowsServiceOptions IgnoreFailureOnServiceStartStop(bool value)
        {
            _values.IgnoreFailureOnServiceStartStop = value;
            return this;
        }

        public IOfferWindowsServiceOptions StartupType(ServiceStartMode type)
        {
            _values.StartupType = type;
            return this;
        }

        public IOfferWindowsServiceOptions DoNotStartAfterInstall()
        {
            _values.DoNotStart = true;
            return this;
        }

        public IOfferWindowsServiceOptions OnServiceFailure(int serviceFailureResetInterval, Action<IOfferWindowsServiceFailureOptions> options)
        {
            _values.ServiceFailureResetInterval = serviceFailureResetInterval;
            var failureOptions = new WindowsServiceFailureOptions();
            options(failureOptions);
            _values.FailureOptions = failureOptions.Values;
            return this;
        }

        public WindowsServiceOptionValues Values { get { return _values; } }

        public class WindowsServiceOptionValues
        {
            private const string SERVICE_CONTROLLER_EXE = @"%windir%\system32\sc.exe";

            public string UserName { get; set; }
            public string Password { get; set; }
            //public string DisplayName { get; set; }
            public string Description { get; set; }
            public string ServiceGroup { get; set; }
            public string ExeParams { get; set; }
            public int? ServiceFailureResetInterval { get; set; }
            public int? ServiceRestartDelay { get; set; }
            public bool IgnoreFailureOnServiceStartStop { get; set; }
            public ServiceStartMode? StartupType { get; set; }
            //public bool HasServiceFailureOptions { get { return ServiceFailureResetInterval.HasValue || ServiceRestartDelay.HasValue; } }
            public bool DoNotStart { get; set; }

            public bool HasServiceGroup
            {
                get { return !string.IsNullOrWhiteSpace(ServiceGroup); }
            }

            public WindowsServiceFailureOptions.WindowsServiceFailureOptionValues FailureOptions { get; set; }


            public string GetServiceFailureCommand(string serviceName)
            {
                var serviceFailureCommand = "";

                if (ServiceFailureResetInterval.HasValue)
                {
                    var command = "";
                    if (!string.IsNullOrWhiteSpace(FailureOptions.FirstFailure.Command))
                    {
                        command = "command= " + FailureOptions.FirstFailure.Command;
                    }
                    else if (!string.IsNullOrWhiteSpace(FailureOptions.SecondFailure.Command))
                    {
                        command = "command= " + FailureOptions.SecondFailure.Command;
                    }
                    else if (!string.IsNullOrWhiteSpace(FailureOptions.SubsequentFailures.Command))
                    {
                        command = "command= " + FailureOptions.SubsequentFailures.Command;
                    }

                    serviceFailureCommand = string.Format("{0} failure \"{1}\" reset= {2} actions= {3} {4}",
                        SERVICE_CONTROLLER_EXE,
                        serviceName,
                        ServiceFailureResetInterval.Value,
                        string.Format("{0}/{1}/{2}", FailureOptions.FirstFailure.Action, FailureOptions.SecondFailure.Action, FailureOptions.SubsequentFailures.Action),
                        command
                        );
                }
                return serviceFailureCommand;
            }

            public string GetServiceConfigCommand(string serviceName)
            {
                var serviceConfigCommand = "";
                if (HasServiceGroup)
                {
                    var groupOption = "group= \"" + ServiceGroup + "\"";

                    serviceConfigCommand = string.Format("{0} config \"{1}\" {2}", SERVICE_CONTROLLER_EXE, serviceName, groupOption);
                }
                return serviceConfigCommand;
            }

        }
    }
}
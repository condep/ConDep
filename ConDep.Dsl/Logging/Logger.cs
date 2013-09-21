using System;
using System.Diagnostics;
using System.ServiceProcess;
using log4net;

namespace ConDep.Dsl.Logging
{
    public class Logger
    {
        private bool? _tcServiceExist;
        private static ILogForConDep _log;// = new Logger().Resolve();

        public static void Initialize(ILogForConDep log)
        {
            _log = log;
        }

        public void AutoResolveLogger()
        {
            if (RunningOnTeamCity)
            {
                Initialize(new TeamCityLogger(LogManager.GetLogger("condep-teamcity")));
            }
            else
            {
                Initialize(new ConsoleLogger(LogManager.GetLogger("condep-default")));
            }
        }

        public static ILogForConDep InternalLogger
        {
            get { return _log; }
        }
      
        private bool RunningOnTeamCity
        {
            get
            {
                if(_tcServiceExist == null)
                {
                    try
                    {
                        var tcService = new ServiceController("TCBuildAgent");
                        _tcServiceExist = tcService.Status == ServiceControllerStatus.Running;
                    }
                    catch
                    {
                        _tcServiceExist = false;
                    }
                }
                return _tcServiceExist.Value;
            }
        }

        public static void Info(string message, params object[] formatArgs)
        {
            _log.Info(message, formatArgs);
        }

        public static void Info(string message, Exception ex, params object[] formatArgs)
        {
            _log.Info(message, ex, formatArgs);
        }

        public static void Warn(string message, params object[] formatArgs)
        {
            _log.Warn(message, formatArgs);
        }

        public static void Warn(string message, Exception ex, params object[] formatArgs)
        {
            _log.Warn(message, ex, formatArgs);
        }

        public static void Error(string message, params object[] formatArgs)
        {
            _log.Error(message, null, formatArgs);
        }

        public static void Error(string message, Exception ex, params object[] formatArgs)
        {
            _log.Error(message, ex, formatArgs);
        }

        public static void Verbose(string message, params object[] formatArgs)
        {
            _log.Verbose(message, formatArgs);
        }

        public static void Verbose(string message, Exception ex, params object[] formatArgs)
        {
            _log.Verbose(message, ex, formatArgs);
        }

        public static void Log(string message, TraceLevel traceLevel, params object[] formatArgs)
        {
            _log.Log(message, traceLevel, formatArgs);
        }

        public static void Log(string message, Exception ex, TraceLevel traceLevel, params object[] formatArgs)
        {
            _log.Log(message, ex, traceLevel, formatArgs);
        }

        public static TraceLevel TraceLevel
        {
            get { return _log.TraceLevel; }
            set { _log.TraceLevel = value; }
        }

        public static ConsoleColor BackgroundColor { get; set; }
        public static ConsoleColor ForegroundColor { get; set; }

        public static void LogSectionStart(string name)
        {
            _log.LogSectionStart(name);
        }

        public static void LogSectionEnd(string name)
        {
            _log.LogSectionEnd(name);
        }

        public static ILogForConDep LogInstance
        {
            get { return _log; }
        }
    }
}
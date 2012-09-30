using System.Diagnostics;
using System.ServiceProcess;
using log4net;
using log4net.Core;

namespace ConDep.Dsl
{
    enum TeamCityMessageStatus
    {
        NORMAL, 
        WARNING, 
        FAILURE, 
        ERROR
    }

    public static class Logger
    {
        private static ILog _log;
        private static bool? _tcServiceExist;
        private static ILog _teamCityServiceMessageLog = LogManager.GetLogger("condep-teamcity-servicemessage");
        
        private static ILog InternalLogger
        {
            get { return _log ?? (_log = LogManager.GetLogger(RunningOnTeamCity ? "condep-teamcity" : "condep-default")); }
        }
        
        public static bool RunningOnTeamCity
        {
            get
            {
                if(_tcServiceExist == null)
                {
                    var log = LogManager.GetLogger("condep-no-time");
                    try
                    {
                        log.Logger.Log(typeof(Logger), Level.All, "Checking if ConDep is running under Team City", null);
                        var tcService = new ServiceController("TCBuildAgent");
                        _tcServiceExist = tcService.Status == ServiceControllerStatus.Running;
                        log.Logger.Log(typeof (Logger), Level.All,
                                       _tcServiceExist.Value
                                           ? "Running on Team City - using Team City formatting"
                                           : "Not running on Team City - using default formatting", null);
                    }
                    catch
                    {
                        log.Logger.Log(typeof(Logger), Level.All, "Not running on Team City - using default formatting", null);
                        _tcServiceExist = false;
                    }
                }
                return _tcServiceExist.Value;
            }
        }

        public static void Info(string message, params object[] formatArgs)
        {
            if(formatArgs == null)
            {
                InternalLogger.Info(message);
            }
            else
            {
                InternalLogger.InfoFormat(message, formatArgs);
            }
        }

        public static void Warn(string message, params object[] formatArgs)
        {
            if (formatArgs == null)
            {
                if(RunningOnTeamCity)
                {
                    TeamCityWarning(message);
                }
                else
                {
                    InternalLogger.Warn(message);
                }
            }
            else
            {
                if (RunningOnTeamCity)
                {
                    TeamCityWarning(message, formatArgs);
                }
                else
                {
                    InternalLogger.WarnFormat(message, formatArgs);
                }
            }
        }

        private static void TeamCityWarning(string message, params object[] formatArgs)
        {
            TeamCityMessage(message, "", TeamCityMessageStatus.WARNING, formatArgs);    
        }

        private static void TeamCityError(string message, string errorDetails, params object[] formatArgs)
        {
            TeamCityMessage(message, errorDetails, TeamCityMessageStatus.WARNING, formatArgs);
        }

        private static void TeamCityMessage(string message, string errorDetails, TeamCityMessageStatus status, params object[] formatArgs)
        {
            if (!RunningOnTeamCity) return;

            var formattedMessage = formatArgs != null ? string.Format(message, formatArgs) : "";
            var tcMessage = string.Format("##teamcity[message text='{0}' errorDetails='{1}' status='{2}']", formattedMessage, errorDetails, status);
            InternalLogger.Logger.Log(typeof(Logger), Level.All, tcMessage, null);
        }

        public static void Error(string message, params object[] formatArgs)
        {
            if (formatArgs == null)
            {
                if(RunningOnTeamCity)
                {
                    TeamCityError(message, "");
                }
                else
                {
                    InternalLogger.Error(message);
                }
            }
            else
            {
                if (RunningOnTeamCity)
                {
                    TeamCityError(message, "", formatArgs);
                }
                else
                {
                    InternalLogger.ErrorFormat(message, formatArgs);
                }
            }
        }

        public static void Verbose(string message, params object[] formatArgs)
        {
            if (formatArgs == null)
            {
                InternalLogger.Debug(message);
            }
            else
            {
                InternalLogger.DebugFormat(message, formatArgs);
            }
        }

        public static void Log(string message, TraceLevel traceLevel)
        {
            var level = GetLog4NetLevel(traceLevel);
            InternalLogger.Logger.Log(typeof(Logger), level, message, null);
        }

        private static Level GetLog4NetLevel(TraceLevel traceLevel)
        {
            switch (traceLevel)
            {
                case TraceLevel.Verbose:
                    return Level.Debug;
                case TraceLevel.Warning:
                    return Level.Warn;
                case TraceLevel.Error:
                    return Level.Error;
                case TraceLevel.Info:
                    return Level.Info;
                case TraceLevel.Off:
                    return Level.Off;
            }
            return Level.Verbose;
        }

        public static TraceLevel TraceLevel
        {
            get
            {
                if (((log4net.Repository.Hierarchy.Logger)InternalLogger.Logger).Level.Name == Level.Debug.Name) return TraceLevel.Verbose;
                if (((log4net.Repository.Hierarchy.Logger)InternalLogger.Logger).Level.Name == Level.Warn.Name) return TraceLevel.Warning;
                if (((log4net.Repository.Hierarchy.Logger)InternalLogger.Logger).Level.Name == Level.Info.Name) return TraceLevel.Info;
                if (((log4net.Repository.Hierarchy.Logger)InternalLogger.Logger).Level.Name == Level.Error.Name) return TraceLevel.Error;
                if (((log4net.Repository.Hierarchy.Logger)InternalLogger.Logger).Level.Name == Level.Off.Name) return TraceLevel.Off;

                return TraceLevel.Verbose;
            }
            set
            {
                switch (value)
                {
                    case TraceLevel.Verbose:
                        ((log4net.Repository.Hierarchy.Logger)InternalLogger.Logger).Level = Level.Debug;
                        break;
                    case TraceLevel.Warning:
                        ((log4net.Repository.Hierarchy.Logger)InternalLogger.Logger).Level = Level.Warn;
                        break;
                    case TraceLevel.Error:
                        ((log4net.Repository.Hierarchy.Logger)InternalLogger.Logger).Level = Level.Error;
                        break;
                    case TraceLevel.Off:
                        ((log4net.Repository.Hierarchy.Logger)InternalLogger.Logger).Level = Level.Off;
                        break;
                    case TraceLevel.Info:
                        ((log4net.Repository.Hierarchy.Logger)InternalLogger.Logger).Level = Level.Info;
                        break;
                }
            }
        }

        public static void TeamCityBlockStart(string name)
        {
            if (!RunningOnTeamCity) return;

            _teamCityServiceMessageLog.Logger.Log(typeof(Logger), Level.All, string.Format("##teamcity[blockOpened name='{0}']", name), null);
        }

        public static void TeamCityBlockEnd(string name)
        {
            if (!RunningOnTeamCity) return;
            _teamCityServiceMessageLog.Logger.Log(typeof(Logger), Level.All, string.Format("##teamcity[blockClosed name='{0}']", name), null);
        }

        public static void TeamCityProgressMessage(string message)
        {
            if (!RunningOnTeamCity) return;
            _teamCityServiceMessageLog.Logger.Log(typeof(Logger), Level.All, string.Format("##teamcity[progressMessage '{0}']", message), null);
        }
    }
}
using System.Diagnostics;
using System.ServiceProcess;
using System.Text;
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
            Log(message, TraceLevel.Info, formatArgs);
        }

        public static void Warn(string message, params object[] formatArgs)
        {
            if(RunningOnTeamCity)
            {
                TeamCityWarning(message, formatArgs);
            }
            else
            {
                Log(message, TraceLevel.Warning, formatArgs);
            }
        }

        private static void TeamCityWarning(string message, params object[] formatArgs)
        {
            TeamCityMessage(message, "", TeamCityMessageStatus.WARNING, formatArgs);    
        }

        private static void TeamCityError(string message, string errorDetails, params object[] formatArgs)
        {
            TeamCityMessage(message, errorDetails, TeamCityMessageStatus.ERROR, formatArgs);
        }

        private static void TeamCityMessage(string message, string errorDetails, TeamCityMessageStatus status, params object[] formatArgs)
        {
            if (!RunningOnTeamCity) return;

            var formattedMessage = (formatArgs != null && formatArgs.Length > 0)? string.Format(message, formatArgs) : message;
            var sb = new StringBuilder(formattedMessage);
            sb.Replace("|", "||")
                .Replace("'", "|'")
                .Replace("\n", "|n")
                .Replace("\r", "|r")
                .Replace("\u0085", "|x")
                .Replace("\u2028", "|l")
                .Replace("\u2029", "|p")
                .Replace("[", "|[")
                .Replace("]", "|]");

            var tcMessage = string.Format("##teamcity[message text='{0}' errorDetails='{1}' status='{2}']", sb, errorDetails, status);
            _teamCityServiceMessageLog.Logger.Log(typeof(Logger), Level.All, tcMessage, null);
        }

        public static void Error(string message, params object[] formatArgs)
        {
            if(RunningOnTeamCity)
            {
                TeamCityError(message, "", formatArgs);
            }
            else
            {
                Log(message, TraceLevel.Error, formatArgs);
            }
        }

        public static void Verbose(string message, params object[] formatArgs)
        {
            Log(message, TraceLevel.Verbose, formatArgs);
        }

        public static void Log(string message, TraceLevel traceLevel, params object[] formatArgs)
        {
            var level = GetLog4NetLevel(traceLevel);
            var formattedMessage = (formatArgs != null && formatArgs.Length > 0 ) ? string.Format(message, formatArgs) : message;
            InternalLogger.Logger.Log(typeof(Logger), level, formattedMessage, null);
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
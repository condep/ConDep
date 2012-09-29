using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
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
        //private static readonly string _teamCityEnvVar = Environment.GetEnvironmentVariable("TEAMCITY_VERSION");

        private static ILog InternalLogger
        {
            get { return _log ?? (_log = LogManager.GetLogger(RunningOnTeamCity ? "condep.teamcity" : "condep.out")); }
        }

        //private static bool RunningOnTeamCity { get { return !string.IsNullOrWhiteSpace(_teamCityEnvVar); } }
        public static bool RunningOnTeamCity
        {
            get
            {
                //todo: need to optimize, so this does not run on every check
                var codeBase = Assembly.GetCallingAssembly().CodeBase;
                var assemblyFullPath = Uri.UnescapeDataString(new UriBuilder(codeBase).Path);
                var assemblyDirectory = Path.GetDirectoryName(assemblyFullPath);
                return assemblyDirectory.ToLowerInvariant().Contains("buildagent\\work");
            }
        }

        public static void Info(string message, params object[] formatArgs)
        {
            if(formatArgs == null)
            {
                InternalLogger.Info(@message);
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
                InternalLogger.Warn(@message);
                TeamCityWarning(message);
            }
            else
            {
                InternalLogger.WarnFormat(message, formatArgs);
                TeamCityWarning(message, formatArgs);
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
                InternalLogger.Error(@message);
                TeamCityError(message, "");
            }
            else
            {
                InternalLogger.ErrorFormat(message, formatArgs);
                TeamCityError(message, "", formatArgs);
            }
        }

        public static void Verbose(string message, params object[] formatArgs)
        {
            if (formatArgs == null)
            {
                InternalLogger.Debug(@message);
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
            InternalLogger.Logger.Log(typeof(Logger), Level.All, string.Format("Adding TeamCity block for '{0}']", name), null);
            InternalLogger.Logger.Log(typeof(Logger), Level.All, string.Format("##teamcity[blockOpened name='{0}']", name), null);
        }

        public static void TeamCityBlockEnd(string name)
        {
            if (!RunningOnTeamCity) return;
            InternalLogger.Logger.Log(typeof(Logger), Level.All, string.Format("Closing TeamCity block for '{0}']", name), null);
            InternalLogger.Logger.Log(typeof(Logger), Level.All, string.Format("##teamcity[blockClosed name='{0}']", name), null);
        }

        public static void TeamCityProgressMessage(string message)
        {
            if (!RunningOnTeamCity) return;
            InternalLogger.Logger.Log(typeof(Logger), Level.All, string.Format("##teamcity[progressMessage '{0}']", message), null);
        }
    }
}
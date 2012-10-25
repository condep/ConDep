using System.Collections;
using Microsoft.Build.Framework;

namespace ConDep.Dsl.Operations.TransformConfig
{
    public class TransformConfigBuildEngine : IBuildEngine
    {
        public void LogErrorEvent(BuildErrorEventArgs e)
        {
            Logger.Error(e.Message);
        }

        public void LogWarningEvent(BuildWarningEventArgs e)
        {
            Logger.Warn(e.Message);
        }

        public void LogMessageEvent(BuildMessageEventArgs e)
        {
            Logger.Info(e.Message);
        }

        public void LogCustomEvent(CustomBuildEventArgs e)
        {
            Logger.Info(e.Message);
        }

        public bool BuildProjectFile(string projectFileName, string[] targetNames, IDictionary globalProperties, IDictionary targetOutputs)
        {
            return true;
        }

        public bool ContinueOnError { get { return true; } }
        public int LineNumberOfTaskNode { get { return 0; } }
        public int ColumnNumberOfTaskNode { get { return 0; } }
        public string ProjectFileOfTaskNode { get { return string.Empty; } }
    }
}
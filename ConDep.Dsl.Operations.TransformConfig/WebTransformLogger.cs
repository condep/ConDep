using System;
using Microsoft.Web.Publishing.Tasks;

namespace ConDep.Dsl
{
    internal class WebTransformLogger : IXmlTransformationLogger
	{
		public void LogMessage(string message, params object[] messageArgs)
		{
            Logger.Verbose(message, messageArgs);
		}

		public void LogMessage(MessageType type, string message, params object[] messageArgs)
		{
            Logger.Verbose(message, messageArgs);
		}

		public void LogWarning(string message, params object[] messageArgs)
		{
            Logger.Warn(message, messageArgs);
		}

		public void LogWarning(string file, string message, params object[] messageArgs)
		{
            Logger.Warn(message, messageArgs);
		}

		public void LogWarning(string file, int lineNumber, int linePosition, string message, params object[] messageArgs)
		{
            Logger.Warn(message, messageArgs);
		}

		public void LogError(string message, params object[] messageArgs)
		{
            Logger.Error(message, messageArgs);
		}

		public void LogError(string file, string message, params object[] messageArgs)
		{
            Logger.Error(message, messageArgs);
		}

		public void LogError(string file, int lineNumber, int linePosition, string message, params object[] messageArgs)
		{
            Logger.Error(message, messageArgs);
		}

		public void LogErrorFromException(Exception ex)
		{
            Logger.Error(ex.Message);
		}

		public void LogErrorFromException(Exception ex, string file)
		{
            Logger.Error(ex.Message);
		}

		public void LogErrorFromException(Exception ex, string file, int lineNumber, int linePosition)
		{
            Logger.Error(ex.Message);
        }

		public void StartSection(string message, params object[] messageArgs)
		{
            Logger.Verbose(message, messageArgs);
		}

		public void StartSection(MessageType type, string message, params object[] messageArgs)
		{
            Logger.Verbose(message, messageArgs);
		}

		public void EndSection(string message, params object[] messageArgs)
		{
            Logger.Verbose(message, messageArgs);
		}

		public void EndSection(MessageType type, string message, params object[] messageArgs)
		{
            Logger.Verbose(message, messageArgs);
		}
	}
}
using System;
using System.Diagnostics;
using ConDep.Dsl.Operations.WebDeploy.Model;
using Microsoft.Web.Publishing.Tasks;

namespace ConDep.Dsl
{
	public class WebTransformLogger : IXmlTransformationLogger
	{
		private readonly EventHandler<WebDeployMessageEventArgs> _output;
		private readonly EventHandler<WebDeployMessageEventArgs> _outputError;

		public WebTransformLogger(EventHandler<WebDeployMessageEventArgs> output, EventHandler<WebDeployMessageEventArgs> outputError)
		{
			_output = output;
			_outputError = outputError;
		}

		public void LogMessage(string message, params object[] messageArgs)
		{
			_output(this, new WebDeployMessageEventArgs{ Level = TraceLevel.Verbose, Message = string.Format(message, messageArgs)});
		}

		public void LogMessage(MessageType type, string message, params object[] messageArgs)
		{
			_output(this, new WebDeployMessageEventArgs { Level = TraceLevel.Verbose, Message = string.Format(message, messageArgs) });
		}

		public void LogWarning(string message, params object[] messageArgs)
		{
			_output(this, new WebDeployMessageEventArgs { Level = TraceLevel.Warning, Message = string.Format(message, messageArgs) });
		}

		public void LogWarning(string file, string message, params object[] messageArgs)
		{
			_output(this, new WebDeployMessageEventArgs { Level = TraceLevel.Warning, Message = string.Format(message, messageArgs) });
		}

		public void LogWarning(string file, int lineNumber, int linePosition, string message, params object[] messageArgs)
		{
			_output(this, new WebDeployMessageEventArgs { Level = TraceLevel.Warning, Message = string.Format(message, messageArgs) });
		}

		public void LogError(string message, params object[] messageArgs)
		{
			_outputError(this, new WebDeployMessageEventArgs { Level = TraceLevel.Error, Message = string.Format(message, messageArgs) });
		}

		public void LogError(string file, string message, params object[] messageArgs)
		{
			_outputError(this, new WebDeployMessageEventArgs { Level = TraceLevel.Error, Message = string.Format(message, messageArgs) });
		}

		public void LogError(string file, int lineNumber, int linePosition, string message, params object[] messageArgs)
		{
			_outputError(this, new WebDeployMessageEventArgs { Level = TraceLevel.Error, Message = string.Format(message, messageArgs) });
		}

		public void LogErrorFromException(Exception ex)
		{
			_outputError(this, new WebDeployMessageEventArgs { Level = TraceLevel.Error, Message = ex.Message});
		}

		public void LogErrorFromException(Exception ex, string file)
		{
			_outputError(this, new WebDeployMessageEventArgs { Level = TraceLevel.Error, Message = ex.Message });
		}

		public void LogErrorFromException(Exception ex, string file, int lineNumber, int linePosition)
		{
			_outputError(this, new WebDeployMessageEventArgs { Level = TraceLevel.Error, Message = ex.Message });
		}

		public void StartSection(string message, params object[] messageArgs)
		{
			_output(this, new WebDeployMessageEventArgs { Level = TraceLevel.Verbose, Message = string.Format(message, messageArgs) });
		}

		public void StartSection(MessageType type, string message, params object[] messageArgs)
		{
			_output(this, new WebDeployMessageEventArgs { Level = TraceLevel.Verbose, Message = string.Format(message, messageArgs) });
		}

		public void EndSection(string message, params object[] messageArgs)
		{
			_output(this, new WebDeployMessageEventArgs { Level = TraceLevel.Verbose, Message = string.Format(message, messageArgs) });
		}

		public void EndSection(MessageType type, string message, params object[] messageArgs)
		{
			_output(this, new WebDeployMessageEventArgs { Level = TraceLevel.Verbose, Message = string.Format(message, messageArgs) });
		}
	}
}
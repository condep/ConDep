using System;
using System.Diagnostics;
using System.Web.Compilation;
using ConDep.Dsl.Operations.WebDeploy.Model;

namespace ConDep.Dsl
{
	public class PreCompileCallback : ClientBuildManagerCallback
	{
		private readonly EventHandler<WebDeployMessageEventArgs> _output;
		private readonly EventHandler<WebDeployMessageEventArgs> _outputError;

		public PreCompileCallback(EventHandler<WebDeployMessageEventArgs> output, EventHandler<WebDeployMessageEventArgs> outputError)
		{
			_output = output;
			_outputError = outputError;
		}

		public override void ReportCompilerError(System.CodeDom.Compiler.CompilerError error)
		{
			var args = new WebDeployMessageEventArgs {Level = TraceLevel.Error, Message = error.ErrorText};
			_outputError(this, args);
		}

		public override void ReportParseError(System.Web.ParserError error)
		{
			var args = new WebDeployMessageEventArgs { Level = TraceLevel.Error, Message = error.ErrorText };
			_outputError(this, args);
		}

		public override void ReportProgress(string message)
		{
			var args = new WebDeployMessageEventArgs { Level = TraceLevel.Verbose, Message = message};
			_output(this, args);
		}
	}
}
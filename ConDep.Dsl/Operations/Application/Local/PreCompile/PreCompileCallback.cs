using System.Web.Compilation;
using ConDep.Dsl.Logging;

namespace ConDep.Dsl.Operations.Application.Local.PreCompile
{
    internal class PreCompileCallback : ClientBuildManagerCallback
	{
		public override void ReportCompilerError(System.CodeDom.Compiler.CompilerError error)
		{
			Logger.Error(error.ErrorText);
		}

		public override void ReportParseError(System.Web.ParserError error)
		{
            Logger.Error(error.ErrorText);
		}

		public override void ReportProgress(string message)
		{
            Logger.Verbose(message);
		}
	}
}
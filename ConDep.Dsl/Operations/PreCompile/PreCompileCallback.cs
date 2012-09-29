using System.Web.Compilation;

namespace ConDep.Dsl.Operations.PreCompile
{
	public class PreCompileCallback : ClientBuildManagerCallback
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
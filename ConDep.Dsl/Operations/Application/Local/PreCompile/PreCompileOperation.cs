using System;
using System.IO;
using System.Web.Compilation;
using ConDep.Dsl.Logging;
using ConDep.Dsl.SemanticModel;

namespace ConDep.Dsl.Operations.Application.Local.PreCompile
{
    public class PreCompileOperation : LocalOperation
	{
		private readonly string _webApplicationName;
		private readonly string _webApplicationPhysicalPath;
		private readonly string _preCompileOutputpath;

		public PreCompileOperation(string webApplicationName, string webApplicationPhysicalPath, string preCompileOutputpath)
		{
			_webApplicationName = webApplicationName;
			_webApplicationPhysicalPath = webApplicationPhysicalPath;
			_preCompileOutputpath = preCompileOutputpath;
		}

        public override IReportStatus Execute(IReportStatus status)
		{
			try
			{
				if(Directory.Exists(_preCompileOutputpath))
					Directory.Delete(_preCompileOutputpath, true);

				var buildManager = new ClientBuildManager(_webApplicationName, _webApplicationPhysicalPath, _preCompileOutputpath, new ClientBuildManagerParameter{ PrecompilationFlags = PrecompilationFlags.Updatable });
				buildManager.PrecompileApplication(new PreCompileCallback());
			}
			catch (Exception ex)
			{
                Logger.Error(ex.Message);
				throw;
			}
			return status;
		}

        public override bool IsValid(Notification notification)
		{
			return true;
		}
	}
}
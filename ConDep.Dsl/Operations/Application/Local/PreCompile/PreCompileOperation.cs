using System;
using System.IO;
using System.Web.Compilation;
using ConDep.Dsl.Config;
using ConDep.Dsl.Logging;
using ConDep.Dsl.SemanticModel;

namespace ConDep.Dsl.Operations.Application.Local.PreCompile
{
    public class PreCompileOperation : LocalOperation
	{
		private readonly string _webApplicationName;
		private readonly string _webApplicationPhysicalPath;
		private readonly string _preCompileOutputpath;
        private readonly IWrapClientBuildManager _buildManager;

        public PreCompileOperation(string webApplicationName, string webApplicationPhysicalPath, string preCompileOutputpath)
		{
			_webApplicationName = webApplicationName;
			_webApplicationPhysicalPath = webApplicationPhysicalPath;
			_preCompileOutputpath = preCompileOutputpath;
            _buildManager = new ClientBuildManagerWrapper(_webApplicationName, _webApplicationPhysicalPath, _preCompileOutputpath, new ClientBuildManagerParameter { PrecompilationFlags = PrecompilationFlags.Updatable });
        }

        public PreCompileOperation(string webApplicationName, string webApplicationPhysicalPath, string preCompileOutputpath, IWrapClientBuildManager buildManager)
        {
            _webApplicationName = webApplicationName;
            _webApplicationPhysicalPath = webApplicationPhysicalPath;
            _preCompileOutputpath = preCompileOutputpath;
            _buildManager = buildManager;
        }

        public override void Execute(IReportStatus status, ConDepSettings settings)
		{
			try
			{
				if(Directory.Exists(_preCompileOutputpath))
					Directory.Delete(_preCompileOutputpath, true);

				_buildManager.PrecompileApplication(new PreCompileCallback());
			}
			catch (Exception ex)
			{
                Logger.Error(ex.Message);
				throw;
			}
		}

        public override bool IsValid(Notification notification)
        {
            return !string.IsNullOrWhiteSpace(_webApplicationName);
        }
	}
}
using System;
using System.Diagnostics;
using System.IO;
using System.Web.Compilation;
using ConDep.Dsl.Builders;
using ConDep.Dsl.Operations.WebDeploy.Model;

namespace ConDep.Dsl
{
	public class PreCompileOperation : IOperateConDep
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

		public WebDeploymentStatus Execute(EventHandler<WebDeployMessageEventArgs> output, EventHandler<WebDeployMessageEventArgs> outputError, WebDeploymentStatus webDeploymentStatus)
		{
			try
			{
				if(Directory.Exists(_preCompileOutputpath))
					Directory.Delete(_preCompileOutputpath, true);

				var buildManager = new ClientBuildManager(_webApplicationName, _webApplicationPhysicalPath, _preCompileOutputpath);
				buildManager.PrecompileApplication(new PreCompileCallback(output, outputError));
			}
			catch (Exception ex)
			{
				var args = new WebDeployMessageEventArgs { Level = TraceLevel.Error, Message = ex.Message };
				outputError(this, args);
				throw;
			}
			return webDeploymentStatus;
		}

		public bool IsValid(Notification notification)
		{
			return true;
		}
	}
}
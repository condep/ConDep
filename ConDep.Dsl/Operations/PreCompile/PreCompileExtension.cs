using ConDep.Dsl.Builders;

namespace ConDep.Dsl
{
	public static class PreCompileExtension
	{
		public static void PreCompile(this DeploymentOptions deploymentOptions, string webApplicationName, string webApplicationPhysicalPath, string preCompileOutputpath)
		{
			var preCompileOperation = new PreCompileOperation(webApplicationName, webApplicationPhysicalPath, preCompileOutputpath);
			deploymentOptions.AddOperation(preCompileOperation);
		}
	}
}
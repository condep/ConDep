using ConDep.Dsl.FluentWebDeploy.Builders;

namespace ConDep.Dsl.FluentWebDeploy
{
	public static class PreCompileExtension
	{
		public static void PreCompile(this SetupBuilder setupBuilder, string webApplicationName, string webApplicationPhysicalPath, string preCompileOutputpath)
		{
			var preCompileOperation = new PreCompileOperation(webApplicationName, webApplicationPhysicalPath, preCompileOutputpath);
			setupBuilder.AddOperation(preCompileOperation);
		}
	}
}
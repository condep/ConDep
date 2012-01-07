using ConDep.Dsl.Builders;

namespace ConDep.Dsl
{
	public static class PreCompileExtension
	{
		public static void PreCompile(this SetupOptions setupOptions, string webApplicationName, string webApplicationPhysicalPath, string preCompileOutputpath)
		{
			var preCompileOperation = new PreCompileOperation(webApplicationName, webApplicationPhysicalPath, preCompileOutputpath);
			setupOptions.AddOperation(preCompileOperation);
		}
	}
}
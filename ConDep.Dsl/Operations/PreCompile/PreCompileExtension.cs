using ConDep.Dsl.Builders;

namespace ConDep.Dsl
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
using ConDep.Dsl.Core;

namespace ConDep.Dsl
{
	public static class PreCompileExtension
	{
		public static void PreCompile(this ISetupCondep conDepSetup, string webApplicationName, string webApplicationPhysicalPath, string preCompileOutputpath)
		{
			var preCompileOperation = new PreCompileOperation(webApplicationName, webApplicationPhysicalPath, preCompileOutputpath);
            ((ConDepSetup)conDepSetup).AddOperation(preCompileOperation);
		}
	}
}
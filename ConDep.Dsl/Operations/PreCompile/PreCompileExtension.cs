using ConDep.Dsl;
using ConDep.Dsl.Operations.PreCompile;

namespace ConDep.Dsl
{
	public static class PreCompileExtension
	{
        public static void PreCompile(this IProvideForSetup conDepSetup, string webApplicationName, string webApplicationPhysicalPath, string preCompileOutputpath)
		{
			var preCompileOperation = new PreCompileOperation(webApplicationName, webApplicationPhysicalPath, preCompileOutputpath);
            ((ConDepSetup)conDepSetup).AddOperation(preCompileOperation);
		}
	}
}
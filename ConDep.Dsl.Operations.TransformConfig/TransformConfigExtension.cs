using ConDep.Dsl;

namespace ConDep.Dsl
{
	public static class TransformConfigExtension
	{
        public static void TransformConfig(this IProvideForSetup conDepSetup, string configDirPath, string configName, string transformName)
		{
			var transformWebConfigOperation = new TransformConfigOperation(configDirPath, configName, transformName);
			((ConDepSetup) conDepSetup).AddOperation(transformWebConfigOperation);
		}

	}
}
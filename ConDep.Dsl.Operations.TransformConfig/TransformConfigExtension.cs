using ConDep.Dsl.Builders;

namespace ConDep.Dsl
{
	public static class TransformConfigExtension
	{
		public static void TransformConfig(this SetupOptions setupOptions, string configDirPath, string configName, string transformName)
		{
			var transformWebConfigOperation = new TransformConfigOperation(configDirPath, configName, transformName);
			setupOptions.AddOperation(transformWebConfigOperation);
		}

	}
}
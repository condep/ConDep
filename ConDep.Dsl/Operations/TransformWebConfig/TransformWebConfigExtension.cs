using ConDep.Dsl.Builders;

namespace ConDep.Dsl
{
	public static class TransformWebConfigExtension
	{
		public static void TransformWebConfig(this SetupOptions setupOptions, string configDirPath, string configName, string transformName)
		{
			var transformWebConfigOperation = new TransformWebConfigOperation(configDirPath, configName, transformName);
			setupOptions.AddOperation(transformWebConfigOperation);
		}

	}
}
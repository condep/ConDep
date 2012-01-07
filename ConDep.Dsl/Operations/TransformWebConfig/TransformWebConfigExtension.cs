using ConDep.Dsl.Builders;

namespace ConDep.Dsl
{
	public static class TransformWebConfigExtension
	{
		public static void TransformWebConfig(this SetupOptions setupOptions, string webConfigDirPath, string buildConfigName)
		{
			var transformWebConfigOperation = new TransformWebConfigOperation(webConfigDirPath, buildConfigName);
			setupOptions.AddOperation(transformWebConfigOperation);
		}

	}
}
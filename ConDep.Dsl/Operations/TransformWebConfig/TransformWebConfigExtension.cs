using ConDep.Dsl.Builders;

namespace ConDep.Dsl
{
	public static class TransformWebConfigExtension
	{
		public static void TransformWebConfig(this SetupBuilder setupBuilder, string webConfigDirPath, string buildConfigName)
		{
			var transformWebConfigOperation = new TransformWebConfigOperation(webConfigDirPath, buildConfigName);
			setupBuilder.AddOperation(transformWebConfigOperation);
		}

	}
}
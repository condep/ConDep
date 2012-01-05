using ConDep.Dsl.FluentWebDeploy.Builders;

namespace ConDep.Dsl.FluentWebDeploy
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
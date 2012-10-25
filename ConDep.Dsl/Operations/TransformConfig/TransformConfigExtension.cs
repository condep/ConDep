using ConDep.Dsl;
using ConDep.Dsl.Operations.TransformConfig;

namespace ConDep.Dsl
{
	public static class TransformConfigExtension
	{
        /// <summary>
        /// Does transformation of .NET configiguration files like web.config and app.config. 
        /// When a configuration file is used for transformation, ConDep will leave behind a [configName].config.orig.condep 
        /// e.g. (web.config.orig.condep) file that will be used if transforms are executed later.
        /// </summary>
        /// <param name="conDepSetup"></param>
        /// <param name="configDirPath">Directory path to where configuration and transformation files are located. Path can be relative.</param>
        /// <param name="configName">Name of the configuration file (e.g. web.config or app.config).</param>
        /// <param name="transformName">Name of the transformation file (e.g. web.test.config)</param>
        public static void TransformConfig(this IProvideForSetup conDepSetup, string configDirPath, string configName, string transformName)
		{
			var transformWebConfigOperation = new TransformConfigOperation(configDirPath, configName, transformName);
            ((ISetupConDep)conDepSetup).AddOperation(transformWebConfigOperation);
		}

	}
}
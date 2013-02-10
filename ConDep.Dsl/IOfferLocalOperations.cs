using System;

namespace ConDep.Dsl
{
    public interface IOfferLocalOperations
    {
        /// <summary>
        /// Transforms .NET configuration files (web and app config), in exactly the same way as msbuild and Visual Studio does.
        /// </summary>
        /// <param name="configDirPath"></param>
        /// <param name="configName"></param>
        /// <param name="transformName"></param>
        /// <returns></returns>
        IOfferLocalOperations TransformConfigFile(string configDirPath, string configName, string transformName);

        /// <summary>
        /// Pre-compile Web Applications to optimize startup time for the application. Even though this operation exist in ConDep, we recommend you to pre-compile web applications as part of your build process, and not the deployment process, using aspnet_compiler.exe.
        /// </summary>
        /// <param name="webApplicationName"></param>
        /// <param name="webApplicationPhysicalPath"></param>
        /// <param name="preCompileOutputpath"></param>
        /// <returns></returns>
        IOfferLocalOperations PreCompile(string webApplicationName, string webApplicationPhysicalPath, string preCompileOutputpath);

        /// <summary>
        /// Executes a simple HTTP GET to the specified url expecting a 200 (OK) in return. Will throw an exception if not 200.
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        IOfferLocalOperations HttpGet(string url);

        /// <summary>
        /// Provide operations to perform on remote servers
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        IOfferRemoteOperations ToEachServer(Action<IOfferRemoteOperations> action);
    }
}
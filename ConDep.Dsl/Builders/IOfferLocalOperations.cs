using System;

namespace ConDep.Dsl.Builders
{
    public interface IOfferLocalOperations
    {
        IOfferLocalOperations TransformConfigFile(string configDirPath, string configName, string transformName);
        IOfferLocalOperations PreCompile(string webApplicationName, string webApplicationPhysicalPath, string preCompileOutputpath);
        IOfferLocalOperations ExecuteWebRequest(string method, string url);
        IOfferRemoteOperations ToEachServer(Action<IOfferRemoteOperations> action);
    }
}
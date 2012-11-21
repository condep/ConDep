using System;

namespace ConDep.Dsl.Experimental.Application
{
    public interface IOfferApplicationOps
    {
        IOfferApplicationOps TransformConfigFile(string configDirPath, string configName, string transformName);
        IOfferApplicationOps PreCompile(string webApplicationName, string webApplicationPhysicalPath, string preCompileOutputpath);
        IOfferApplicationOps ExecuteWebRequest(string method, string url);
        IOfferRemoteOptions ToEachServer(Action<IOfferRemoteOptions> action);
    }
}
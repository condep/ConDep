namespace ConDep.Dsl.Application
{
    public interface IExecuteLocally
    {
        void TransformConfigFile(string configDirPath, string configName, string transformName);
        void PreCompile(string webApplicationName, string webApplicationPhysicalPath, string preCompileOutputpath);
        void ExecuteWebRequest(string method, string url);
    }
}
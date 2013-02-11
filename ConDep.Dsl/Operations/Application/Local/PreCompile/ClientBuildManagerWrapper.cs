using System.Web.Compilation;

namespace ConDep.Dsl.Operations.Application.Local.PreCompile
{
    public class ClientBuildManagerWrapper : IWrapClientBuildManager
    {
        private readonly ClientBuildManager _buildManager;
        public ClientBuildManagerWrapper(string webApplicationName, string webApplicationPhysicalPath, string preCompileOutputpath, ClientBuildManagerParameter parameter)
        {
            _buildManager = new ClientBuildManager(webApplicationName, webApplicationPhysicalPath, preCompileOutputpath, parameter);
        }

        public void PrecompileApplication(ClientBuildManagerCallback callback)
        {
            _buildManager.PrecompileApplication(callback);
        }
    }
}
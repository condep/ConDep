using System.Web.Compilation;

namespace ConDep.Dsl.Operations.Application.Local.PreCompile
{
    public interface IWrapClientBuildManager
    {
        void PrecompileApplication(ClientBuildManagerCallback callback);
    }
}
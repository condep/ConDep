using ConDep.Dsl.Config;
using ConDep.Dsl.Logging;
using ConDep.Dsl.Remote.Node;
using ConDep.Dsl.SemanticModel;

namespace ConDep.Dsl.Operations.Application.Deployment.CopyDir
{
    public class CopyDirOperation : IOperateRemote
    {
        private readonly string _srcDir;
        private readonly string _dstDir;
        private Api _api;

        public CopyDirOperation(string srcDir, string dstDir)
        {
            _srcDir = srcDir;
            _dstDir = dstDir;
        }

        public bool IsValid(Notification notification)
        {
            return true;
        }

        public void Execute(ServerConfig server, IReportStatus status, ConDepSettings settings)
        {
            _api = new Api(string.Format("http://{0}/ConDepNode/", server.Name), server.DeploymentUser.UserName, server.DeploymentUser.Password);
            var result = _api.SyncDir(_srcDir, _dstDir);

            if (result == null) return;
            
            if(result.Log.Count > 0)
            {
                foreach (var entry in result.Log)
                {
                    Logger.Info(entry);
                }
            }
            else
            {
                Logger.Info("Nothing to deploy. Everything is in sync.");
            }
        }

        public string Name { get { return "Copy Dir"; } }
    }
}
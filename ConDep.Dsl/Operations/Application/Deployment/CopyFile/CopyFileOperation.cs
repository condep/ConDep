using ConDep.Dsl.Config;
using ConDep.Dsl.Logging;
using ConDep.Dsl.SemanticModel;
using ConDep.Node.Client;

namespace ConDep.Dsl.Operations.Application.Deployment.CopyFile
{
    public class CopyFileOperation : IOperateRemote
    {
        private readonly string _srcFile;
        private readonly string _dstFile;
        private Api _api;

        public CopyFileOperation(string srcFile, string dstFile)
        {
            _srcFile = srcFile;
            _dstFile = dstFile;
        }

        public void Execute(ServerConfig server, IReportStatus status, ConDepSettings settings)
        {
            _api = new Api(string.Format("http://{0}/ConDepNode/", server.Name), server.DeploymentUser.UserName, server.DeploymentUser.Password);
            var result = _api.SyncFile(_srcFile, _dstFile);

            if (result == null) return;

            if (result.Log.Count > 0)
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

        public bool IsValid(Notification notification)
        {
            return true;
        }
    }
}
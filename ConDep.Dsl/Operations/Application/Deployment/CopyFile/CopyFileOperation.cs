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
            _api = new Api(string.Format("http://{0}/ConDepNode/", server.Name));
            var result = _api.SyncFile(_srcFile, _dstFile);

            foreach (var entry in result.Log)
            {
                Logger.Info(entry);
            }

            Logger.Info(
    @"Sync result:

    Files Created       : {0}
    Files Updated       : {3}
    Files Deleted       : {2}
    Directories Deleted : {1}
", result.CreatedFiles.Count, result.DeletedDirectories.Count, result.DeletedFiles.Count, result.UpdatedFiles.Count);
        }

        public bool IsValid(Notification notification)
        {
            return true;
        }
    }
}
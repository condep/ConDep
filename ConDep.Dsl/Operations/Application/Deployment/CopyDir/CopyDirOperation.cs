using ConDep.Dsl.Config;
using ConDep.Dsl.Logging;
using ConDep.Dsl.SemanticModel;
using ConDep.Node.Client;

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

        //public override string Name
        //{
        //    get { return "SyncDir"; }
        //}

        public bool IsValid(Notification notification)
        {
            return true;
        }

        public void Execute(ServerConfig server, IReportStatus status, ConDepSettings settings)
        {
            _api = new Api(string.Format("http://{0}/ConDepNode/", server.Name));
            var result = _api.SyncDir(_srcDir, _dstDir);

            Logger.Info(
    @"Sync result:

    Files Created       : {0}
    Files Updated       : {3}
    Files Deleted       : {2}
    Directories Deleted : {1}
", result.CreatedFiles, result.DeletedDirectories, result.DeletedFiles, result.UpdatedFiles);
        }
    }
}
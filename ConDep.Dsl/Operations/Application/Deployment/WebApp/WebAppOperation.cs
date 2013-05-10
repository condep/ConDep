using ConDep.Dsl.Config;
using ConDep.Dsl.Logging;
using ConDep.Dsl.SemanticModel;
using ConDep.Node.Client;

namespace ConDep.Dsl.Operations.Application.Deployment.WebApp
{
    public class WebAppOperation : IOperateRemote
    {
        private readonly string _sourceDir;
        private readonly string _webAppName;
        private readonly string _destinationWebSiteName;
        private readonly string _destDir;
        private Api _api;

        public WebAppOperation(string sourceDir, string webAppName, string destinationWebSiteName, string destDir = null)
        {
            _sourceDir = sourceDir;
            _webAppName = webAppName;
            _destinationWebSiteName = destinationWebSiteName;
            _destDir = destDir;
        }

        public bool IsValid(Notification notification)
        {
            return true;
        }

        public void Execute(ServerConfig server, IReportStatus status, ConDepSettings settings)
        {
            _api = new Api(string.Format("http://{0}/ConDepNode/", server.Name));
            var result = _api.SyncWebApp(_destinationWebSiteName, _webAppName, _sourceDir, _destDir);

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
    }
}
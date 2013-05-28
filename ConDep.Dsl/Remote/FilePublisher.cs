using ConDep.Dsl.Config;
using ConDep.Dsl.Logging;
using ConDep.Dsl.Remote.Node;

namespace ConDep.Dsl.Remote
{
    public class FilePublisher
    {
        public void PublishFile(string srcFile, string dstFile, ServerConfig server)
        {
            var api = new Api(string.Format("http://{0}/ConDepNode/", server.Name), server.DeploymentUser.UserName, server.DeploymentUser.Password);
            var result = api.SyncFile(srcFile, dstFile);

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

        public void PublishDirectory(string srcDir, string dstDir, ServerConfig server)
        {
            var api = new Api(string.Format("http://{0}/ConDepNode/", server.Name), server.DeploymentUser.UserName, server.DeploymentUser.Password);
            var result = api.SyncDir(srcDir, dstDir);

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
    }
}
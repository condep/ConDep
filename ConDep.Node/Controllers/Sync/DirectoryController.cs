using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using ConDep.Node.Model;

namespace ConDep.Node.Controllers.Sync
{
    public class DirectoryController : ApiController
    {
        private readonly SyncDirHandler _dirHandler;
        private readonly PathValidator _pathValidator;

        public DirectoryController()
        {
            _dirHandler = new SyncDirHandler();
            _pathValidator = new PathValidator();
        }

        public DirectoryController(SyncDirHandler dirHandler, PathValidator pathValidator)
        {
            _dirHandler = dirHandler;
            _pathValidator = pathValidator;
        }

        public SyncDirDirectory Get(string path)
        {
            path = Environment.ExpandEnvironmentVariables(path);
            if (!_pathValidator.ValidPath(path))
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.InternalServerError, "Invalid path. Path cannot be root of drive, or any of the common system and windows folders."));
            }

            var dirUrl = ApiUrls.Sync.DirectoryTemplate(Url);
            var fileUrl = ApiUrls.Sync.FileTemplate(Url);

            var dir = _dirHandler.GetSubDirInfo(path, dirUrl, fileUrl, new DirectoryInfo(path));
            return dir;
        }

        public Task<HttpResponseMessage> Put(string path)
        {
            path = Environment.ExpandEnvironmentVariables(path);
            return SyncDir(path);
        }

        public Task<HttpResponseMessage> Post(string path)
        {
            path = Environment.ExpandEnvironmentVariables(path);
            return SyncDir(path);
        }

        private Task<HttpResponseMessage> SyncDir(string path)
        {
            if (!_pathValidator.ValidPath(path))
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError,
                                                                            "Invalid path. Path cannot be root of drive, or any of the common system and windows folders."));
            }
            if (!Request.Content.IsMimeMultipartContent("dir-sync-data"))
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.UnsupportedMediaType));

            var streamProvider = new MultipartSyncDirStreamProvider(path);

            return Request.Content.ReadAsMultipartAsync(streamProvider)
                .ContinueWith(t => Request.CreateResponse(HttpStatusCode.Created, t.Result.SyncResult));
        }
    }
}
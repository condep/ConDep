using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using ConDep.Node.Model;

namespace ConDep.Node.Controllers
{
    public class FileController : ApiController
    {
        private readonly SyncDirHandler _dirHandler;

        public FileController()
        {
            _dirHandler = new SyncDirHandler();
        }

        public FileController(SyncDirHandler dirHandler)
        {
            _dirHandler = dirHandler;
        }

        public SyncDirFile Get(string path)
        {
            var fileUrl = ApiUrls.Sync.FileTemplate(Url);
            var dirUrl = ApiUrls.Sync.DirectoryTemplate(Url);

            var file = _dirHandler.GetFileInfo(path, fileUrl, dirUrl, new FileInfo(path));
            return file;
        }

        public Task<HttpResponseMessage> Post(string path, long lastWriteTimeUtc, string fileAttributes)
        {
            if(File.Exists(path))
            {
                throw new HttpResponseException(
                    Request.CreateErrorResponse(
                        HttpStatusCode.Found,
                        "File already exist. Use PUT to update existing file. Event better, follow the rules of HATEOAS and this will never happen.")
                    );
            }

            return Request.Content.ReadAsStreamAsync().ContinueWith(t =>
                                                                        {
                                                                            CreateFile(path, lastWriteTimeUtc, t.Result, fileAttributes);
                                                                            return Request.CreateResponse(HttpStatusCode.Created, new SyncResult{CreatedFiles = 1});
                                                                        });
        }

        private static void CreateFile(string path, long lastWriteTimeUtc, Stream requestStream, string fileAttributes)
        {
            var file = new FileInfo(path);
            Directory.CreateDirectory(file.DirectoryName);

            StreamToFile(path, requestStream, file, lastWriteTimeUtc, fileAttributes);
        }

        private static void UpdateFile(string path, long lastWriteTimeUtc, Stream requestStream, string fileAttributes)
        {
            var file = new FileInfo(path);
            file.Attributes = FileAttributes.Normal;
            file.Delete();

            StreamToFile(path, requestStream, file, lastWriteTimeUtc, fileAttributes);
        }

        public static void StreamToFile(string path, Stream requestStream, FileInfo file, long lastWriteTimeUtc, string fileAttributes)
        {
            var fileStream = File.Create(path);
            requestStream.CopyTo(fileStream);
            fileStream.Close();
            requestStream.Close();

            file.Refresh();
            var dateTime = DateTime.FromFileTime(lastWriteTimeUtc);

            FileAttributes attributes;
            if (!FileAttributes.TryParse(fileAttributes, true, out attributes))
            {
                throw new Exception("");
            }

            file.LastWriteTimeUtc = dateTime;
            file.Attributes = attributes;
        }

        public HttpResponseMessage Delete(string path)
        {
            var fileInfo = new FileInfo(path);
            if (!fileInfo.Exists)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            fileInfo.Attributes = FileAttributes.Normal;
            fileInfo.Delete();
            return Request.CreateResponse(HttpStatusCode.NoContent);
        }

        public Task<HttpResponseMessage> Put(string path, long lastWriteTimeUtc, string fileAttributes)
        {
            if(!File.Exists(path))
            {
                throw new HttpResponseException(
                    Request.CreateErrorResponse(
                        HttpStatusCode.Found,
                        "File does not exist. Use POST to create this file. Event better, follow the rules of HATEOAS and this will never happen.")
                    );
            }

            return Request.Content.ReadAsStreamAsync().ContinueWith(t =>
            {
                UpdateFile(path, lastWriteTimeUtc, t.Result, fileAttributes);
                return Request.CreateResponse(HttpStatusCode.Created, new SyncResult { UpdatedFiles = 1 });
            });
        }
    }
}
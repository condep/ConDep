using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ConDep.Client.Controllers
{
    public class FileController : ApiController
    {
        public HttpResponseMessage Post(string path, long lastWriteTimeUtc, string fileAttributes)
        {
            if(File.Exists(path))
            {
                throw new HttpResponseException(
                    Request.CreateErrorResponse(
                        HttpStatusCode.Found, 
                        "File already exist. Use PUT to update existing file.")
                    );
            }

            var task = Request.Content.ReadAsStreamAsync();
            task.Wait();
            var requestStream = task.Result;

            try
            {
                CreateFile(path, lastWriteTimeUtc, requestStream, fileAttributes);
            }
            catch (IOException)
            {
                throw new HttpResponseException(HttpStatusCode.InternalServerError);
            }

            return new HttpResponseMessage { StatusCode = HttpStatusCode.Created };
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

        public HttpResponseMessage Put(string path, long lastWriteTimeUtc, string fileAttributes)
        {
            if(!File.Exists(path))
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            var task = Request.Content.ReadAsStreamAsync();
            task.Wait();
            var requestStream = task.Result;

            try
            {
                UpdateFile(path, lastWriteTimeUtc, requestStream, fileAttributes);
            }
            catch (IOException)
            {
                throw new HttpResponseException(HttpStatusCode.InternalServerError);
            }

            return new HttpResponseMessage { StatusCode = HttpStatusCode.OK };
        }
    }
}
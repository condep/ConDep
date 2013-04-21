using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ConDep.Client
{
    public class DirectoryController : ApiController
    {
        public HttpResponseMessage Get(string dirPath)
        {
            if(Directory.Exists(dirPath))
            {
                var syncDirHandler = new SyncDirHandler();
                var uri = Request.RequestUri;

                var dirUrl = String.Format("{0}{1}{2}{3}", uri.Scheme, Uri.SchemeDelimiter, uri.Authority, uri.AbsolutePath);
                var fileUrl = dirUrl.Replace("Directory/", "File/");

                var obj = syncDirHandler.GetSubDirInfo(dirUrl, fileUrl, new DirectoryInfo(dirPath));
                return Request.CreateResponse(HttpStatusCode.OK, obj);
            }
            return Request.CreateResponse(HttpStatusCode.NotFound);
        }
    }

    public class FileController : ApiController
    {
        public HttpResponseMessage Post(string path)
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
                var fileStream = File.Create(path);
                requestStream.CopyTo(fileStream);
                fileStream.Close();
                requestStream.Close();
            }
            catch (IOException)
            {
                throw new HttpResponseException(HttpStatusCode.InternalServerError);
            }

            return new HttpResponseMessage { StatusCode = HttpStatusCode.Created };
        }

        public HttpResponseMessage Delete(string path)
        {
            if (!File.Exists(path))
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            File.Delete(path);
            return Request.CreateResponse(HttpStatusCode.NoContent);
        }

        public HttpResponseMessage Put(string path)
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
                var fileStream = File.Create(path);
                requestStream.CopyTo(fileStream);
                fileStream.Close();
                requestStream.Close();
            }
            catch (IOException)
            {
                throw new HttpResponseException(HttpStatusCode.InternalServerError);
            }

            return new HttpResponseMessage { StatusCode = HttpStatusCode.OK };
        }
    }
}
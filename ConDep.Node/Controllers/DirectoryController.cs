using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ConDep.Client.Controllers
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
}
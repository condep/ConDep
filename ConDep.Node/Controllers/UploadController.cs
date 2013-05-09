using System.IO;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ConDep.Node.Controllers
{
    public class UploadController : ApiController
    {
        public HttpResponseMessage Post(string filename)
        {
            var task = Request.Content.ReadAsStreamAsync();
            task.Wait();
            var requestStream = task.Result;

            try
            {
                var fileStream = File.Create(@"C:\temp\" + filename);
                requestStream.CopyTo(fileStream);
                fileStream.Close();
                requestStream.Close();
            }
            catch(IOException)
            {
                throw new HttpResponseException(HttpStatusCode.InternalServerError);
            }

            return new HttpResponseMessage {StatusCode = HttpStatusCode.Created};
        }
    }
}
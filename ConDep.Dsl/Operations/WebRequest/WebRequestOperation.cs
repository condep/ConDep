using System.Net;
using ConDep.Dsl.Experimental.Core;

namespace ConDep.Dsl.Operations.WebRequest
{
    public class WebRequestOperation : LocalOperation
    {
        private readonly string _url;
        private readonly string _method;

        public WebRequestOperation(string url, string method)
        {
            _url = url;
            _method = method;
        }

        public override bool IsValid(Notification notification)
        {
            return !string.IsNullOrWhiteSpace(_url) || !string.IsNullOrWhiteSpace(_method);
        }

        public override IReportStatus Execute(IReportStatus status)
        {
            var webRequest = System.Net.WebRequest.Create(_url);
            webRequest.Method = _method;
            webRequest.ContentLength = 0;
            webRequest.ContentType = "application/x-www-form-urlencoded";

            ServicePointManager.ServerCertificateValidationCallback = (sender, certificate, chain, errors) => true;

            HttpStatusCode statusCode = ((HttpWebResponse)webRequest.GetResponse()).StatusCode;

            if (statusCode == HttpStatusCode.OK)
            {
                Logger.Info("HTTP {0} Succeeded: {1}", _method.ToUpper(), _url);
            }
            else
            {
                Logger.Error("HTTP {0} Failed with Status {1}: {2}", _method.ToUpper(), statusCode, _url);
            }

            return status;
        }
    }
}
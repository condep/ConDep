using System;
using System.Net;
using System.Threading;
using ConDep.Dsl.Config;
using ConDep.Dsl.Logging;
using ConDep.Dsl.SemanticModel;

namespace ConDep.Dsl.Operations.Application.Local.WebRequest
{
    public class HttpGetOperation : LocalOperation
    {
        private readonly string _url;

        public HttpGetOperation(string url)
        {
            _url = url;
        }

        public override bool IsValid(Notification notification)
        {
            return !string.IsNullOrWhiteSpace(_url) && Uri.IsWellFormedUriString(_url, UriKind.Absolute);
        }

        public override void Execute(IReportStatus status, ConDepSettings settings)
        {
            Thread.Sleep(1000);
            var webRequest = System.Net.WebRequest.Create(_url);
            webRequest.Method = "GET";
            webRequest.ContentLength = 0;
            webRequest.ContentType = "application/x-www-form-urlencoded";

            ServicePointManager.ServerCertificateValidationCallback = (sender, certificate, chain, errors) => true;

            HttpStatusCode statusCode = ((HttpWebResponse)webRequest.GetResponse()).StatusCode;

            if (statusCode == HttpStatusCode.OK)
            {
                Logger.Info("HTTP {0} Succeeded: {1}", "GET", _url);
            }
            else
            {
                throw new WebException(string.Format("GET request did not return with 200 (OK), but {0} ({1})", (int)statusCode, statusCode));
            }
        }

        public override string Name
        {
            get { return "Http Get"; }
        }
    }
}
using System;
using System.Diagnostics;
using System.Net;
using ConDep.Dsl.WebDeploy;

namespace ConDep.Dsl.Operations.WebRequest
{
    public class WebRequestOperation : ConDepOperationBase
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
            return true;
        }

        public override WebDeploymentStatus Execute(TraceLevel traceLevel, EventHandler<WebDeployMessageEventArgs> output, EventHandler<WebDeployMessageEventArgs> outputError, WebDeploymentStatus webDeploymentStatus)
        {
            var webRequest = System.Net.WebRequest.Create(_url);
            webRequest.Method = _method;
            webRequest.ContentLength = 0;
            webRequest.ContentType = "application/x-www-form-urlencoded";

            ServicePointManager.ServerCertificateValidationCallback = (sender, certificate, chain, errors) => true;

            HttpStatusCode statusCode = ((HttpWebResponse)webRequest.GetResponse()).StatusCode;

            if (statusCode == HttpStatusCode.OK)
            {
                var args = new WebDeployMessageEventArgs { Level = TraceLevel.Info, Message = string.Format("HTTP {0} Succeeded: {1}", _method.ToUpper(), _url) };
                output(this, args);
            }
            else
            {
                var args = new WebDeployMessageEventArgs { Level = TraceLevel.Error, Message = string.Format("HTTP {0} Failed with Status {1}: {2}", _method.ToUpper(), statusCode, _url) };
                outputError(this, args);
            }

            return webDeploymentStatus;
        }
    }
}
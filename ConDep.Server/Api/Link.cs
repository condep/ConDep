using System.Net.Http;

namespace ConDep.Server.Api
{
    public class Link
    {
        public HttpMethod Method { get; set; }
        public string Rel { get; set; }
        public string Href { get; set; }
    }
}
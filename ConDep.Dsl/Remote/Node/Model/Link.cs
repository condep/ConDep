using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace ConDep.Dsl.Remote.Node.Model
{

    public class Link : IEquatable<Link>
    {
        private IEnumerable<Link> _links = new List<Link>();

        public string Rel { get; set; }
        public string Href { get; set; }
        public IEnumerable<Link> Links
        {
            get { return _links; }
            set { _links = value; }
        }

        public string Method { get; set; }

        public HttpMethod HttpMethod
        {
            get
            {
                switch (Method)
                {
                    case "DELETE": return HttpMethod.Delete;
                    case "GET": return HttpMethod.Get;
                    case "HEAD": return HttpMethod.Head;
                    case "OPTIONS": return HttpMethod.Options;
                    case "POST": return HttpMethod.Post;
                    case "PUT": return HttpMethod.Put;
                    case "TRACE": return HttpMethod.Trace;
                    default : throw new NotSupportedException(Method);
                }
            }
        }

        public bool Equals(Link other)
        {
            if (other == null) return false;
            if (Rel != other.Rel) return false;
            if (Href != other.Href) return false;
            if (Method != other.Method) return false;
            if (Links.Intersect(other.Links).Any()) return false;
            return true;
        }
    }

    public static class LinkListExtensions
    {
        public static Link GetByRel(this List<Link> listOfLinks, string rel)
        {
            return listOfLinks.SingleOrDefault(link => link.Rel == rel);
        } 
    }
}
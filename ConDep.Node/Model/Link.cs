using System.Collections.Generic;

namespace ConDep.Node.Model
{

    public class Link 
    {
        private IEnumerable<Link> _links = new List<Link>();

        public string Method { get; set; }
        public string Rel { get; set; }
        public string Href { get; set; }
        public IEnumerable<Link> Links
        {
            get { return _links; }
            set { _links = value; }
        }
    }
}
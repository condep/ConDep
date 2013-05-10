using System.Collections.Generic;

namespace ConDep.Node.Model
{
    public class WebAppInfo
    {
        private List<Link> _links = new List<Link>();

        public List<Link> Links { get { return _links; } }

        public string PhysicalPath { get; set; }

        public bool Exist { get; set; }
    }
}
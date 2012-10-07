using System.Collections.Generic;

namespace ConDep.Dsl.Model.Config
{
    public class WebSiteConfig
    {
        public string Name { get; set; }
        public IList<WebSiteBindingConfig> Bindings { get; set; }
    }
}
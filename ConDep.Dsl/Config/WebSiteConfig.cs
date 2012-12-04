using System.Collections.Generic;

namespace ConDep.Dsl.Config
{
    public class WebSiteConfig
    {
        public string Name { get; set; }
        public IList<WebSiteBindingConfig> Bindings { get; set; }
    }
}
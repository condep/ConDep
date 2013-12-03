using System;
using System.Collections.Generic;

namespace ConDep.Dsl.Config
{
    [Serializable]
    public class WebSiteConfig
    {
        public string Name { get; set; }
        public IList<WebSiteBindingConfig> Bindings { get; set; }
    }
}
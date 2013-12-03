using System;

namespace ConDep.Dsl.Config
{
    [Serializable]
    public class WebSiteBindingConfig
    {
        public string BindingType { get; set; }
        public string Port { get; set; }
        public string Ip { get; set; }
        public string HostHeader { get; set; }
    }
}
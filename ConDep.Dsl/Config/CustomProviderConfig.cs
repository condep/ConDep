using System.Collections.Generic;

namespace ConDep.Dsl.Config
{
    public class CustomProviderConfig
    {
        public string ProviderName { get; set; }
        public Dictionary<string, string> ProviderConfig { get; set; }
    }
}
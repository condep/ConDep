namespace ConDep.Dsl.Config
{
    public class CustomProviderConfig
    {
        public string ProviderName { get; set; }
        public CustomJsonConfigDictionary<string, string> ProviderConfig { get; set; }
    }
}
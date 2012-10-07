namespace ConDep.Dsl.Model.Config
{
    public class CustomProviderConfig
    {
        public string ProviderName { get; set; }
        public CustomJsonConfigDictionary<string, string> ProviderConfig { get; set; }
    }
}
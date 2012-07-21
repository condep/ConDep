namespace ConDep.Dsl.Core
{
    public class LoadBalancerSettings
    {
        public bool IsDefined { get { return !string.IsNullOrWhiteSpace(Name) && !string.IsNullOrWhiteSpace(Provider); } }
        public string Name { get; set; }
        public string Provider { get; set; }
    }
}
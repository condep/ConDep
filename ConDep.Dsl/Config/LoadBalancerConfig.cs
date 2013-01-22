using System;

namespace ConDep.Dsl.Config
{
    public class LoadBalancerConfig
    {
        public string Name { get; set; }
        public string Provider { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Mode { get; set; }
        public LbMode ModeAsEnum
        {
            get
            {
                switch (Mode.ToLower())
                {
                    case "sticky":
                        return LbMode.Sticky;
                    case "roundrobin":
                        return LbMode.RoundRobin;
                    default:
                        throw new NotSupportedException(string.Format("Load Balancer Mode [{0}] is not supported.", Mode));
                }
            }
        }
    }

    public enum LbMode
    {
        RoundRobin,
        Sticky
    }
}
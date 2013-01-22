using System;
using System.Collections.Generic;
using ConDep.Dsl.Config;
using ConDep.Dsl.Operations.LoadBalancer;
using ConDep.Dsl.SemanticModel;

namespace ConDep.Dsl.Tests
{
    public class MockLoadBalancer : ILoadBalance
    {
        private readonly List<Tuple<string, string>> _onlineOfflineSequence = new List<Tuple<string, string>>();

        public void BringOffline(string serverName, string farm, LoadBalancerSuspendMethod suspendMethod, IReportStatus status)
        {
            _onlineOfflineSequence.Add(new Tuple<string,string>(serverName, "offline"));
        }

        public void BringOnline(string serverName, string farm, IReportStatus status)
        {
            _onlineOfflineSequence.Add(new Tuple<string, string>(serverName, "online"));
        }

        public LbMode Mode { get; set; }

        public IList<Tuple<string, string>> OnlineOfflineSequence { get { return _onlineOfflineSequence; } }
    }
}
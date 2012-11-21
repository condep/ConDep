using ConDep.Dsl.Experimental.Core;

namespace ConDep.Dsl.LoadBalancer
{
    public interface ILoadBalance
    {
        void BringOffline(string serverName, IReportStatus status);
        void BringOnline(string serverName, IReportStatus status);
    }
}
using System;
using ConDep.Dsl.Operations.ApplicationRequestRouting;
using ConDep.Dsl.Operations.ApplicationRequestRouting.Infrastructure;

namespace ConDep.Dsl
{
    //public class ApplicationRequestRoutingOptions
    //{
    //    private readonly ApplicationReqeustRoutingLoadBalancer _arrLoadBalancer;
    //    private readonly ArrLoadBalancerOptions _loadBalancer;

    //    public ApplicationRequestRoutingOptions(ApplicationReqeustRoutingLoadBalancer arrLoadBalancer)
    //    {
    //        _arrLoadBalancer = arrLoadBalancer;
    //        _loadBalancer = new ArrLoadBalancerOptions(arrLoadBalancer);
    //    }

    //    //public ApplicationRequestRoutingOptions TakeFarmOfflineForServer(string serverIp, string farmName)
    //    //{
    //    //   _arrOperation.AddServer(farmName, serverIp, ServerState.Offline);
    //    //   return this;
    //    //}

    //    //public ApplicationRequestRoutingOptions TakeAllFarmsOfflineForServer(string serverName)
    //    //{
    //    //   throw new NotImplementedException();
    //    //}

    //    //public ApplicationRequestRoutingOptions TakeFarmOnlineForServer(string serverIp, string farmName)
    //    //{
    //    //   _arrOperation.AddServer(farmName, serverIp, ServerState.Online);
    //    //   return this;
    //    //}

    //    //public ApplicationRequestRoutingOptions TakeAllFarmsOnlineForServer(string serverName)
    //    //{
    //    //   throw new NotImplementedException();
    //    //}
    //    //public ArrLoadBalancerOptions LoadBalancer
    //    //{
    //    //    get {
    //    //        return _loadBalancer;
    //    //    }
    //    //}
    //}

    //public class ArrLoadBalancerOptions
    //{
    //    private readonly ApplicationReqeustRoutingLoadBalancer _arrLoadBalancer;

    //    public ArrLoadBalancerOptions(ApplicationReqeustRoutingLoadBalancer arrLoadBalancer)
    //    {
    //        _arrLoadBalancer = arrLoadBalancer;
    //    }

    //    public FarmOptions Farm(string farmName)
    //    {
    //        return new FarmOptions(farmName, _arrLoadBalancer);
    //    }
    //}

    //public class FarmOptions
    //{
    //    private readonly string _farmName;
    //    private readonly ApplicationReqeustRoutingLoadBalancer _arrLoadBalancer;

    //    public FarmOptions(string farmName, ApplicationReqeustRoutingLoadBalancer arrLoadBalancer)
    //    {
    //        _farmName = farmName;
    //        _arrLoadBalancer = arrLoadBalancer;
    //    }

    //    public void TakeServerOffline(string nodeIp)
    //    {
    //        _arrLoadBalancer.AddServer(_farmName, nodeIp, ServerState.Offline);
    //    }

    //    public void TakeServerOnline(string nodeIp)
    //    {
    //        _arrLoadBalancer.AddServer(_farmName, nodeIp, ServerState.Online);
    //    }
    //}
}
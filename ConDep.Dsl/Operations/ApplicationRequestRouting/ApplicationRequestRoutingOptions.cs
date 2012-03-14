using System;
using ConDep.Dsl.Operations.ApplicationRequestRouting;
using ConDep.Dsl.Operations.ApplicationRequestRouting.Infrastructure;

namespace ConDep.Dsl
{
	public class ApplicationRequestRoutingOptions
	{
		private readonly ApplicationReqeustRoutingOperation _arrOperation;
		private readonly ArrLoadBalancerOptions _loadBalancer;

		public ApplicationRequestRoutingOptions(ApplicationReqeustRoutingOperation arrOperation)
		{
			_arrOperation = arrOperation;
			_loadBalancer = new ArrLoadBalancerOptions(arrOperation);
		}

		//public ApplicationRequestRoutingOptions TakeFarmOfflineForServer(string serverIp, string farmName)
		//{
		//   _arrOperation.AddServer(farmName, serverIp, ServerState.Offline);
		//   return this;
		//}

		//public ApplicationRequestRoutingOptions TakeAllFarmsOfflineForServer(string serverName)
		//{
		//   throw new NotImplementedException();
		//}

		//public ApplicationRequestRoutingOptions TakeFarmOnlineForServer(string serverIp, string farmName)
		//{
		//   _arrOperation.AddServer(farmName, serverIp, ServerState.Online);
		//   return this;
		//}

		//public ApplicationRequestRoutingOptions TakeAllFarmsOnlineForServer(string serverName)
		//{
		//   throw new NotImplementedException();
		//}
		public ArrLoadBalancerOptions LoadBalancer
		{
			get {
				return _loadBalancer;
			}
		}
	}

	public class ArrLoadBalancerOptions
	{
		private readonly ApplicationReqeustRoutingOperation _arrOperation;

		public ArrLoadBalancerOptions(ApplicationReqeustRoutingOperation arrOperation)
		{
			_arrOperation = arrOperation;
		}

		public FarmOptions Farm(string farmName)
		{
			return new FarmOptions(farmName, _arrOperation);
		}
	}

	public class FarmOptions
	{
		private readonly string _farmName;
		private readonly ApplicationReqeustRoutingOperation _arrOperation;

		public FarmOptions(string farmName, ApplicationReqeustRoutingOperation arrOperation)
		{
			_farmName = farmName;
			_arrOperation = arrOperation;
		}

		public void TakeServerOffline(string nodeIp)
		{
			_arrOperation.AddServer(_farmName, nodeIp, ServerState.Offline);
		}

		public void TakeServerOnline(string nodeIp)
		{
			_arrOperation.AddServer(_farmName, nodeIp, ServerState.Online);
		}
	}
}
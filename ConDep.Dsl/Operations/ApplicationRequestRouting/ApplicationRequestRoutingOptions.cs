using System;
using ConDep.Dsl.Operations.ApplicationRequestRouting;
using ConDep.Dsl.Operations.ApplicationRequestRouting.Infrastructure;

namespace ConDep.Dsl
{
	public class ApplicationRequestRoutingOptions
	{
		private readonly ApplicationReqeustRoutingOperation _arrOperation;

		public ApplicationRequestRoutingOptions(ApplicationReqeustRoutingOperation arrOperation)
		{
			_arrOperation = arrOperation;
		}

		public ApplicationRequestRoutingOptions TakeFarmOfflineForServer(string serverIp, string farmName)
		{
			_arrOperation.AddServer(farmName, serverIp, ServerState.Offline);
			return this;
		}

		public ApplicationRequestRoutingOptions TakeAllFarmsOfflineForServer(string serverName)
		{
			throw new NotImplementedException();
		}

		public ApplicationRequestRoutingOptions TakeFarmOnlineForServer(string serverIp, string farmName)
		{
			_arrOperation.AddServer(farmName, serverIp, ServerState.Online);
			return this;
		}

		public ApplicationRequestRoutingOptions TakeAllFarmsOnlineForServer(string serverName)
		{
			throw new NotImplementedException();
		}
	}
}
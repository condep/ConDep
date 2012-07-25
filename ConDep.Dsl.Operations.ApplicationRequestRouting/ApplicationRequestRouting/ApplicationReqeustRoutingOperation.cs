using System;
using System.Collections.Generic;
using System.Linq;
using ConDep.Dsl.Core;
using ConDep.Dsl.Operations.ApplicationRequestRouting.Infrastructure;

namespace ConDep.Dsl.Operations.ApplicationRequestRouting
{
	public class ApplicationReqeustRoutingOperation : IOperateConDep, ILoadBalance
	{
		private readonly string _arrServerName;
	    private readonly string _serverToDeployTo;
	    private readonly string _serverToBringBackOnline;
	    private readonly UserInfo _userInfo;
		private readonly ArrFarmManager _arrFarmManager = new ArrFarmManager();
		private readonly List<Farm> _farms = new List<Farm>();

		public ApplicationReqeustRoutingOperation(string arrServerName, string serverToDeployTo, string serverToBringBackOnline)
			: this(arrServerName, serverToDeployTo, serverToBringBackOnline, null)
		{
		}

        public ApplicationReqeustRoutingOperation(string arrServerName, string serverToDeployTo, string serverToBringBackOnline, UserInfo userInfo)
		{
			_arrServerName = arrServerName;
            _serverToDeployTo = serverToDeployTo;
            _serverToBringBackOnline = serverToBringBackOnline;
            _userInfo = userInfo;
		}

		public WebDeploymentStatus Execute(EventHandler<WebDeployMessageEventArgs> output, EventHandler<WebDeployMessageEventArgs> outputError, WebDeploymentStatus webDeploymentStatus)
		{
			foreach(var farm in _farms)
			{
				switch(farm.ServerState)
				{
					case ServerState.Offline:
						_arrFarmManager.TakeOffline(_arrServerName, farm.FarmName, farm.ServerIp, _userInfo);
						break;
					case ServerState.Online:
						_arrFarmManager.TakeOnline(_arrServerName, farm.FarmName, farm.ServerIp, _userInfo);
						break;
					default:
						throw new NotImplementedException(string.Format("State {0} is not supported.", farm.ServerState));
				}
				
			}
			return webDeploymentStatus;
		}

		public void AddServer(string farmName, string serverIp, ServerState serverState)
		{
			_farms.Add(new Farm{FarmName = farmName, ServerIp = serverIp, ServerState = serverState});
		}

		internal class Farm
		{
			public string FarmName { get; set; }
			public string ServerIp { get; set; }
			public ServerState ServerState { get; set; }
		}

		public bool IsValid(Notification notification)
		{
			return !string.IsNullOrWhiteSpace(_arrServerName)
                   && _farms.Any(x => !string.IsNullOrWhiteSpace(x.FarmName) && !string.IsNullOrWhiteSpace(x.ServerIp));
		}
	}
}
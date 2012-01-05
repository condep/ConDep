using System;
using System.Collections.Generic;
using ConDep.Dsl.FluentWebDeploy.Builders;
using ConDep.Dsl.FluentWebDeploy.Operations.WebDeploy.Model;
using ConDep.Dsl.FluentWebDeploy.PreSyncOperations.ApplicationRequestRouting.Infrastructure;
using ConDep.Dsl.FluentWebDeploy.SemanticModel;

namespace ConDep.Dsl.FluentWebDeploy
{
	public class ApplicationReqeustRoutingOperation : IOperateConDep
	{
		private readonly string _webServerName;
		private readonly ArrFarmManager _arrFarmManager = new ArrFarmManager();
		private readonly List<Farm> _farms = new List<Farm>();

		public ApplicationReqeustRoutingOperation(string webServerName)
		{
			_webServerName = webServerName;
		}

		public WebDeploymentStatus Execute(EventHandler<WebDeployMessageEventArgs> output, EventHandler<WebDeployMessageEventArgs> outputError, WebDeploymentStatus webDeploymentStatus)
		{
			foreach(var farm in _farms)
			{
				switch(farm.ServerState)
				{
					case ServerState.Offline:
						_arrFarmManager.TakeOffline(_webServerName, farm.FarmName, farm.ServerIp);
						break;
					case ServerState.Online:
						_arrFarmManager.TakeOnline(_webServerName, farm.FarmName, farm.ServerIp);
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
			throw new NotImplementedException();
		}
	}
}
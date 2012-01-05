using System.Threading;

namespace ConDep.Dsl.Operations.ApplicationRequestRouting.Infrastructure
{
	public class ArrFarmManager
	{
		public void TakeOnline(string arrServer, string farmName, string serverIp)
		{
			var arr = new ArrServerManager(arrServer, farmName, serverIp);
			arr.StateExecutor.BringServerOnline();
			arr.Commit();

			arr = new ArrServerManager(arrServer, farmName, serverIp);
			arr.StateExecutor.MakeServerAvailable();
			arr.Commit();
		}

		public void TakeOnline(string arrServer, string farmName, string serverIp, UserInfo user)
		{
			using (new Impersonator(user))
			{
				TakeOnline(arrServer, farmName, serverIp);
			}
		}

		public void TakeOffline(string arrServer, string farmName, string serverIp)
		{
			var arr = new ArrServerManager(arrServer, farmName, serverIp);
			arr.StateExecutor.MakeServerUnavailableGracefully();
			arr.Commit();

			while (arr.Counters.CurrentRequests > 0)
			{
				Thread.Sleep(500);
			}

			arr = new ArrServerManager(arrServer, farmName, serverIp);
			arr.StateExecutor.TakeServerOffline();
			arr.Commit();
		}

		public void TakeOffline(string arrServer, string farmName, string serverIp, UserInfo user)
		{
			using (new Impersonator(user))
			{
				TakeOffline(arrServer, farmName, serverIp);
			}
		}
	}
}
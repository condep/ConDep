using Microsoft.Web.Administration;

namespace ConDep.Dsl.Operations.ApplicationRequestRouting.Infrastructure
{
	public class StateExecutor
	{
		private readonly ConfigurationMethodInstance _method;
		private readonly ConfigurationElement _server;

		public StateExecutor(ConfigurationElement server)
		{
			_server = server;
			_method = _server.ChildElements["applicationRequestRouting"].Methods["SetState"].CreateInstance();
		}

		public bool MakeServerUnavailableGracefully()
		{
			return Execute("GracefulStop");
		}

		public bool MakeServerUnavailable()
		{
			return Execute("ForcefulStop");
		}

		public bool DisallowNewConnections()
		{
			return Execute("Drain");
		}

		public bool MakeServerAvailable()
		{
			return Execute("Start");
		}

		public bool TakeServerOffline()
		{
			_server.SetAttributeValue("enabled", false);
			return true;
		}

		private bool Execute(string state)
		{
			try
			{
				_method.Input.SetAttributeValue("newState", state);
				_method.Execute();
				return true;
			}
			catch { return false; }
		}

		public void BringServerOnline()
		{
			_server.SetAttributeValue("enabled", true);
		}
	}
}
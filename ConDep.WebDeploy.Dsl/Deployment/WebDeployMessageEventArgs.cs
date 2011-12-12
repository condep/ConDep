using System;
using System.Diagnostics;

namespace ConDep.WebDeploy.Dsl.Deployment
{
	public class WebDeployMessageEventArgs : EventArgs
	{
		public string Message { get; set; }
		public TraceLevel Level { get; set; }
	}
}
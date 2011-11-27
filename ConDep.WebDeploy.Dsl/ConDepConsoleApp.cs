using System;
using System.Collections.Generic;
using System.Security;

namespace ConDep.WebDeploy.Dsl
{
	public class ConDepConsoleApp<ConsoleOwner> : WebDeployOperation where ConsoleOwner : ConDepConsoleApp<ConsoleOwner>, new()
	{
		protected CustomConfiguration CustomConfig { get; private set; }

		public static ConsoleOwner Initialize(string[] args)
		{
			string commandLine;

			try
			{
				commandLine = Environment.CommandLine;
			}
			catch (SecurityException exception)
			{
				throw new Exception("Insufficient permissions to run exe", exception);
			}

			var list = CmdParameterParser.ParseOnSpace(commandLine);
			var customConfig = GetCustomConfiguration(list);

			return new ConsoleOwner {CustomConfig = customConfig};
		}

		private static CustomConfiguration GetCustomConfiguration(List<string> list)
		{
			return new CustomConfiguration();
		}
	}

	public class CustomConfiguration
	{
	}
}
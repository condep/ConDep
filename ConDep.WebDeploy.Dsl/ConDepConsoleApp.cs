using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;

namespace ConDep.WebDeploy.Dsl
{
	public class ConDepConsoleApp<TConsoleOwner, TSettings> : WebDeployOperation 
		where TConsoleOwner : ConDepConsoleApp<TConsoleOwner, TSettings>, new()
		where TSettings : ConDepConfiguration, new()
	{
		protected TSettings Settings { get; private set; }

		public static TConsoleOwner Initialize(string[] args)
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

			var list = CmdParameterParser.GetCmdParameters(commandLine);

			if(list.Count == 0)
			{
				PrintHelp();
			}

			foreach (var element in list.Where(element => (element.Equals("-?") || element.Equals("/?")) || element.Equals("-help", StringComparison.OrdinalIgnoreCase)))
			{
				PrintHelp();
			} 

			var customConfig = GetCustomConfiguration(list);

			return new TConsoleOwner {Settings = customConfig};
		}

		private static TSettings GetCustomConfiguration(List<string> list)
		{
			return new TSettings();
		}

		private static void PrintHelp()
		{
			
		}
	}
}
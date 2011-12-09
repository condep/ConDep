using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Security;
using System.Reflection;
using System.Collections.Generic;
using ConDep.WebDeploy.Dsl.Console;
using ConDep.WebDeploy.Dsl.SemanticModel;

namespace ConDep.WebDeploy.Dsl
{
	public abstract class ConDepConsoleApp<TConsoleOwner, TSettings> : WebDeployOperation 
		where TConsoleOwner : ConDepConsoleApp<TConsoleOwner, TSettings>, new()
		where TSettings : ConDepConfiguration, new()
	{

		protected TSettings Settings { get; private set; }

		protected ConDepConsoleApp() : base(new WebDeployDefinition(), new WebDeploy()) 
		{
			Settings = new TSettings();

			var cmdParams = GetCmdParams();
			if (UserNeedsHelp(cmdParams))
			{
				PrintHelp();
				return;
			}

			AddSettingsFromCmdLine(cmdParams);

			Execute();
		}

		private void AddSettingsFromCmdLine(IEnumerable<string> cmdParams)
		{
			var settingParams = ExtractSettingsParams(cmdParams);
			foreach(var param in settingParams)
			{
				Settings.GetType().GetProperty(param.ParamName).SetValue(Settings, param.ParamValue, null);
			}
		}

		private IEnumerable<CmdParam> ExtractSettingsParams(IEnumerable<string> cmdParams)
		{
			return cmdParams.
				Where(p => p.Contains('=')).
				Select(param =>
				       	{
				       		var paramNameValue = param.Split('=');
				       		return new CmdParam
				       		       	{
				       		       		ParamName = paramNameValue[0],
				       		       		ParamValue = paramNameValue[1]
				       		       	};
				       	});
		}

		private static bool UserNeedsHelp(IEnumerable<string> cmdParams)
		{
			return cmdParams.Any(x =>
										x.Equals("/?") ||
										x.Equals("-?") || 
			                     x.Equals("-help", StringComparison.OrdinalIgnoreCase) ||
			                     x.Equals("--help", StringComparison.OrdinalIgnoreCase));
		}

		private static IEnumerable<string> GetCmdParams()
		{
			return CmdParameterParser.GetCmdParameters(GetCmdLine());
		}

		private static string GetCmdLine()
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
			return commandLine;
		}

		protected static void Initialize(string[] args)
		{
			new TConsoleOwner();
		}

		private void PrintHelp()
		{
			System.Console.WriteLine(string.Format("\n" +
			                                "{0} [settingsFile] {1}"
													  , FileName, GetCmdHelpForSettings()));
		}

		private StringBuilder GetCmdHelpForSettings()
		{
			var validSettings = new StringBuilder();
			foreach (var setting in Settings.GetType().GetProperties().Where(setting => setting.IsDefined(typeof (CmdParamAttribute), false)))
			{
				validSettings.Append("[" + setting.Name + "=value] ");
			}
			return validSettings;
		}

		private string FileName
		{
			get
			{
				var file = Assembly.GetEntryAssembly().Location;
				return Path.GetFileName(file);
			}
		}

		protected abstract void Execute();

		public override void OnWebDeployMessage(object sender, WebDeployMessegaEventArgs e)
		{
			if(e.Level == System.Diagnostics.TraceLevel.Error)
			{
				var currentConsoleColor = System.Console.ForegroundColor;
				System.Console.ForegroundColor = ConsoleColor.Yellow;
				System.Console.Error.WriteLine(e.Message);
				System.Console.ForegroundColor = currentConsoleColor;
			}
			else if(e.Level == System.Diagnostics.TraceLevel.Warning)
			{
				var currentConsoleColor = System.Console.ForegroundColor;
				System.Console.ForegroundColor = ConsoleColor.Red;
				System.Console.Out.WriteLine(e.Message);
				System.Console.ForegroundColor = currentConsoleColor;
			}
			else
			{
				System.Console.Out.WriteLine(e.Message);
			}
		}
	}

	internal class CmdParam
	{
		public string ParamName { get; set; }
		public string ParamValue { get; set; }
	}
}
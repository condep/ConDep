using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Security;
using System.Reflection;
using System.Collections.Generic;
using ConDep.Dsl.Console;
using ConDep.Dsl;
using ConDep.Dsl.Operations.WebDeploy.Model;

namespace ConDep.Dsl
{
	//ToDo: Refactor -> Violates SRP (...and probably quite a few other things)
	public abstract class ConDepConsoleApp<TConsoleOwner, TSettings> : ConDepOperation 
		where TConsoleOwner : ConDepConsoleApp<TConsoleOwner, TSettings>, new()
		where TSettings : ConDepConfiguration, new()
	{

		protected ConDepConsoleApp() 
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

		protected TSettings Settings { get; private set; }

		private void AddSettingsFromCmdLine(IEnumerable<string> cmdParams)
		{
			var settingParams = ExtractSettingsParams(cmdParams);
			foreach(var param in settingParams)
			{
				Settings.GetType().GetField(param.ParamName).SetValue(Settings, param.ParamValue);
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
		                                           "{0} {1}"
		                                           , FileName, GetCmdHelpForSettings()));
		}

		private StringBuilder GetCmdHelpForSettings()
		{
			var validSettings = new StringBuilder();
			foreach (var setting in Settings.GetType().GetFields().Where(setting => setting.IsDefined(typeof (CmdParamAttribute), false)))
			{
			    var attrib = setting.GetCustomAttributes(typeof (CmdParamAttribute), false).FirstOrDefault() as CmdParamAttribute;
			    string settingHelp;

                if(attrib == null || !attrib.Mandatory)
                {
                    settingHelp = string.Format("{0}=value ", setting.Name);
                }
                else
                {
                    settingHelp = string.Format("[{0}=value] ", setting.Name);
                }
			    validSettings.Append(settingHelp);
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

		protected override void OnMessage(object sender, WebDeployMessageEventArgs e)
		{
			if(e.Level == System.Diagnostics.TraceLevel.Warning)
			{
				var currentConsoleColor = System.Console.ForegroundColor;
				System.Console.ForegroundColor = ConsoleColor.Yellow;
				System.Console.Out.WriteLine(e.Message);
				System.Console.ForegroundColor = currentConsoleColor;
			}
			else
			{
				System.Console.Out.WriteLine(e.Message);
			}
		}

		protected override void OnErrorMessage(object sender, WebDeployMessageEventArgs e)
		{
			var currentConsoleColor = System.Console.ForegroundColor;
			System.Console.ForegroundColor = ConsoleColor.Red;
			System.Console.Error.WriteLine(e.Message);
			System.Console.ForegroundColor = currentConsoleColor;
		}

	}

	internal sealed class CmdParam
	{
		public string ParamName { get; set; }
		public string ParamValue { get; set; }
	}
}
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Security;
using System.Reflection;
using System.Collections.Generic;
using ConDep.Dsl.Console;
using ConDep.Dsl.Operations.WebDeploy.Model;

namespace ConDep.Dsl
{
	//ToDo: Refactor -> Violates SRP (...and probably quite a few other things)
	public abstract class ConDepConsoleApp<TConsoleOwner, TSettings> : ConDepOperation 
		where TConsoleOwner : ConDepConsoleApp<TConsoleOwner, TSettings>, new()
		where TSettings : class
	{
	    protected ConDepConsoleApp() 
		{
			var cmdParams = GetCmdParams();
            var cmdParams2 = ExtractSettingsParams(cmdParams);

	        var env = GetEnvFromCmdLine(cmdParams2);

	        //Should support not by convention as well. E.g. concrete class.
            var settings = CreateEnvSettingsByConvention(env);

            if (UserNeedsHelp(cmdParams))
			{
				PrintHelp(settings);
				return;
			}

            AddSettingsFromCmdLine(cmdParams, settings);

			Execute(settings);
		}

	    private TSettings CreateEnvSettingsByConvention(string env)
	    {
	        var interfaceType = typeof (TSettings);

            if(!interfaceType.IsInterface)
            {
                throw new ArgumentException(string.Format("Not an interface [{0}].", interfaceType.Name));
            }

            return GetImplementersOfInterfaceForEnvironment(interfaceType, env);
	    }

	    private static TSettings GetImplementersOfInterfaceForEnvironment(Type interfaceType, string env)
	    {
	        var implementers = AppDomain.CurrentDomain.GetAssemblies()
	            .SelectMany(assembly => assembly.GetTypes())
	            .Where(type =>
	                   interfaceType.IsAssignableFrom(type)
	                   &&
                       type.IsClass
                       &&
	                   type.Name.StartsWith(env, true, CultureInfo.InvariantCulture)
	            ).ToList();

            if (implementers == null || implementers.Count() == 0)
            {
                throw new ArgumentException(string.Format("No implementor of the interface [{0}] starting with [{1}] was found.", interfaceType.Name, env));
            }

            if (implementers.Count() > 1)
            {
                throw new ArgumentException(string.Format("More than one implementor of the interface [{0}] starting with [{1}] was found.", interfaceType.Name, env));
            }

	        return implementers.First() as TSettings;
	    }

	    private string GetEnvFromCmdLine(IEnumerable<CmdParam> cmdParams)
	    {
	        var param = cmdParams.FirstOrDefault(x => x.ParamName == "env");
            if(param == null)
            {
                throw new ArgumentNullException("Command line argument for environment [env] is missing.");
            }

	        return param.ParamValue;
	    }

	    protected static void Initialize(string[] args)
        {
            new TConsoleOwner();
        }

		private void AddSettingsFromCmdLine(IEnumerable<string> cmdParams, object settings)
		{
			var settingParams = ExtractSettingsParams(cmdParams);
			foreach(var param in settingParams)
			{
				settings.GetType().GetField(param.ParamName).SetValue(settings, param.ParamValue);
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

		private void PrintHelp(TSettings settings)
		{
		    System.Console.WriteLine(string.Format("\n" +
		                                           "{0} {1}"
		                                           , FileName, GetCmdHelpForSettings(settings)));
		}

		private StringBuilder GetCmdHelpForSettings(TSettings settings)
		{
			var validSettings = new StringBuilder();
			foreach (var setting in settings.GetType().GetFields().Where(setting => setting.IsDefined(typeof (CmdParamAttribute), false)))
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

		protected abstract void Execute(TSettings setting);

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
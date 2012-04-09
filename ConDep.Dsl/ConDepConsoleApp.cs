using System;
using System.IO;
using System.Text;
using System.Reflection;
using ConDep.Dsl.Console;
using ConDep.Dsl.Operations.WebDeploy.Model;

namespace ConDep.Dsl
{
	public abstract class ConDepConsoleApp<TConsoleOwner, TSettings> : ConDepOperation 
		where TConsoleOwner : ConDepConsoleApp<TConsoleOwner, TSettings>, new()
		where TSettings : class
	{
	    protected ConDepConsoleApp() 
		{
			var cmdParams = CmdLineHandler.GetCmdParams();
            var settingsParams = CmdLineHandler.ExtractSettingsParams(cmdParams);
	        var env = CmdLineHandler.GetEnvFromCmdLine(settingsParams);
            var settings = SettingsHandler.CreateInstanceOfEnvironmentSettings<TSettings>(env);

            if (CmdLineHandler.UserNeedsHelp(cmdParams))
			{
				PrintHelp(settings);
				return;
			}

            SettingsHandler.AddSettingsFromCmdLine(settingsParams, settings);

			Execute(settings);
		}

	    protected static void Initialize(string[] args)
        {
            new TConsoleOwner();
        }

		private static void PrintHelp(TSettings settings)
		{
            var file = Assembly.GetEntryAssembly().Location;
            var fileName = Path.GetFileName(file);
            
            System.Console.WriteLine(string.Format("\n" +
		                                           "{0} {1}"
		                                           , fileName, SettingsHandler.GetCmdHelpForSettings(settings)));
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
}
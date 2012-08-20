using System;
using System.Diagnostics;
using System.IO;

namespace ConDep.Dsl.Core
{
	public abstract class ConDepConfigurator
	{
	    private readonly Notification _notification;
		private readonly ConDepSetup _conDepSetup;
	    private TraceLevel _traceLevel = TraceLevel.Info;

	    protected ConDepConfigurator()
		{
		    _conDepSetup = new ConDepSetup();
		    _notification = new Notification();
		}

	    public static ConDepEnvironmentSettings EnvSettings { get; set; }

	    public TraceLevel TraceLevel
	    {
	        get {
	            return _traceLevel;
	        }
	        set {
	            _traceLevel = value;
	        }
	    }

	    //Todo: Must be able to redirect output
	    protected virtual void OnMessage(object sender, WebDeployMessageEventArgs e)
	    {
            if (e.Level == TraceLevel.Warning)
            {
                WriteColorMessage(e, ConsoleColor.Yellow, Console.Out);
            }
            else if(e.Level == TraceLevel.Info)
            {
                WriteColorMessage(e, ConsoleColor.Green, Console.Out);
            }
            else
            {
                Console.Out.WriteLine(DateTime.Now.ToLongTimeString() + " - " + e.Message);
            }
        }

        //Todo: Must be able to redirect output
        protected virtual void OnErrorMessage(object sender, WebDeployMessageEventArgs e)
		{
            WriteColorMessage(e, ConsoleColor.Red, Console.Error);
        }

	    private static void WriteColorMessage(WebDeployMessageEventArgs e, ConsoleColor color, TextWriter writer)
	    {
	        var currentConsoleColor = Console.ForegroundColor;
	        Console.ForegroundColor = color;
	        writer.WriteLine(DateTime.Now.ToLongTimeString() + " - " + e.Message);
	        Console.ForegroundColor = currentConsoleColor;
	    }

	    protected internal WebDeploymentStatus Setup(Action<SetupOptions> action)
		{
			var status = new WebDeploymentStatus();

			action(new SetupOptions(_conDepSetup));
			if (!_conDepSetup.IsValid(_notification))
			{
				_notification.Throw();
			}

			_conDepSetup.Execute(TraceLevel, OnMessage, OnErrorMessage, status);
			
			return status;
		}

	    protected internal abstract void Configure();
	}
}
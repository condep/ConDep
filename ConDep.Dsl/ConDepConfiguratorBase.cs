using System;
using System.Diagnostics;
using System.IO;
using StructureMap;

namespace ConDep.Dsl.Core
{
	public abstract class ConDepConfiguratorBase
	{
	    private TraceLevel _traceLevel = TraceLevel.Info;

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

	    protected internal WebDeploymentStatus Setup(Action<IProvideForSetup> action)
		{
			var status = new WebDeploymentStatus();
	        var conDepSetup = ObjectFactory.GetInstance<ISetupCondep>();
            var notification = new Notification();

            action((IProvideForSetup)conDepSetup);
			if (!conDepSetup.IsValid(notification))
			{
				notification.Throw();
			}

			conDepSetup.Execute(TraceLevel, OnMessage, OnErrorMessage, status);
			
			return status;
		}

	    protected internal abstract void Configure();
	}
}
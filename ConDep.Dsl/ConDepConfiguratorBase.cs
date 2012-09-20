using System;
using System.Diagnostics;
using System.IO;
using ConDep.Dsl.WebDeploy;
using TinyIoC;

namespace ConDep.Dsl
{
	public abstract class ConDepConfiguratorBase
	{
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

	    protected internal void Setup(Action<IProvideForSetup> action)
		{
	        var conDepSetup = TinyIoCContainer.Current.Resolve<ISetupConDep>();
            var notification = new Notification();

            action((IProvideForSetup)conDepSetup);

			if (!conDepSetup.IsValid(notification))
			{
				notification.Throw();
			}

            conDepSetup.Execute(Options, OnMessage, OnErrorMessage, Status);
		}

        protected internal ConDepOptions Options { get; set; }

	    public WebDeploymentStatus Status { get; set; }

        protected internal ConDepEnvironmentSettings EnvSettings { get; set; }

	    protected internal abstract void Configure();
	}
}
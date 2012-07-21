using System;

namespace ConDep.Dsl.Core
{
	public abstract class ConDepConfigurator
	{
	    private readonly Notification _notification;
		private readonly SetupOperation _setupOperation;

		protected ConDepConfigurator()
		{
		    _setupOperation = new SetupOperation();
		    _notification = new Notification();
		}

	    public static ConDepEnvironmentSettings EnvSettings { get; set; }

	    //Todo: Must be able to redirect output
	    protected virtual void OnMessage(object sender, WebDeployMessageEventArgs e)
	    {
            if (e.Level == System.Diagnostics.TraceLevel.Warning)
            {
                var currentConsoleColor = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Out.WriteLine(e.Message);
                Console.ForegroundColor = currentConsoleColor;
            }
            else
            {
                Console.Out.WriteLine(e.Message);
            }
        }

        //Todo: Must be able to redirect output
        protected virtual void OnErrorMessage(object sender, WebDeployMessageEventArgs e)
		{
            var currentConsoleColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Error.WriteLine(e.Message);
            Console.ForegroundColor = currentConsoleColor;
        }

		protected internal WebDeploymentStatus Setup(Action<SetupOptions> action)
		{
			var status = new WebDeploymentStatus();

			action(new SetupOptions(_setupOperation));
			if (!_setupOperation.IsValid(_notification))
			{
				_notification.Throw();
			}

			_setupOperation.Execute(OnMessage, OnErrorMessage, status);
			
			return status;
		}

	    protected internal abstract void Execute();
	}
}
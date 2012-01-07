using System;
using ConDep.Dsl.Builders;
using ConDep.Dsl.Operations.WebDeploy.Model;
using FluentAutomation.API;
using FluentAutomation.API.Providers;

namespace ConDep.Dsl
{
    public static class SmokeTestExtension
    {
        public static void SmokeTest(this SetupOptions setupOptions, Action<SmokeTestOptions> action)
        {
            var smokeTestOperation = new SmokeTestOperation();
            setupOptions.AddOperation(smokeTestOperation);

            action(new SmokeTestOptions());
        }
    }

    public class SmokeTestOptions : FluentTest
    {
        public SmokeTestOptions()
        {
        }

        public void Url(string url)
        {
            I.Open(url);
        }

        public override CommandManager I
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }
    }

    public class SmokeTestOperation : IOperateConDep
    {
        public bool IsValid(Notification notification)
        {
            throw new NotImplementedException();
        }

        public WebDeploymentStatus Execute(EventHandler<WebDeployMessageEventArgs> output, EventHandler<WebDeployMessageEventArgs> outputError, WebDeploymentStatus webDeploymentStatus)
        {
            throw new NotImplementedException();
        }
    }
}
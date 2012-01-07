using System;
using System.Diagnostics;
using ConDep.Dsl.Builders;
using ConDep.Dsl.Operations.WebDeploy.Model;
using FluentAutomation.API.Enumerations;
using FluentAutomation.SeleniumWebDriver;

namespace ConDep.Dsl
{
    public class SmokeTestOperation : FluentTest, IOperateConDep
    {
        private readonly string _url;
        
        public SmokeTestOperation(string url)
        {
            _url = url;
        }

        public bool IsValid(Notification notification)
        {
            return true;
        }

        public WebDeploymentStatus Execute(EventHandler<WebDeployMessageEventArgs> output, EventHandler<WebDeployMessageEventArgs> outputError, WebDeploymentStatus webDeploymentStatus)
        {
            try
            {
                I.Use(BrowserType.Chrome);
                I.Open(_url);
                I.Finish();
            }
            catch(Exception ex)
            {
                var args = new WebDeployMessageEventArgs {Level = TraceLevel.Error, Message = ex.Message};
                outputError(this, args);
                I.Finish();
            }
            return webDeploymentStatus;
        }
    }
}
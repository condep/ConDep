using System.Web.Http;

namespace ConDep.Server.Api.Controllers
{
    public class ApplicationDeploymentController : ApiController
    {
        /*
         
         Post       => Deploy and Application
         Put        => Create a new Application for deployment
         Get        => List Applications or give info about a single Application
         Delete     => Delete an Application
         
         An Application can be configured to be part of either:
              * Pipeline or
              * Environment
         
         A Pipeline is a set of ordered Environments with conditions to execute.
         A Pipeline can be defined globally (accessable to all applications) or
         Application specific (only available to that Application)
         
         A Pipeline has the folowing structure:
        
              Id : [Guid],
              Name: [string],
              Steps : 
              [
                    {
                        Name: [string],
                        Type: [Environment, Custom],
                        Trigger: [Auto, Manual, Condition, Schedule]
                        TimeoutInMinutes: [int]
                    },
                    {
                        ...
                    }
              ],
              CreatedUtc : [DateTime],
              LastModifiedUtc : [DateTime],
              CreatedBy : [string],
              LastModifiedBy : [string]
        */      

        public string Get(string appName)
        {
            return "hello " + appName + "!";
        }         
    }
}
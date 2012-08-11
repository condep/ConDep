using System;
using System.Management.Automation;
using Lapointe.PowerShell.MamlGenerator.Attributes;

namespace ConDep.PowerShell.ApplicationRequestRouting
{
    public delegate void WritePowerShellObject(object obj);

    [Cmdlet(VerbsCommon.Set, "WebFarmServerState"),
     CmdletDescription("Change state of Web Farm Servers."),
     RelatedCmdlets(typeof(GetWebFarmServerCommand))]
    public class SetWebFarmServerStateCommand : Cmdlet
    {
        protected override void ProcessRecord()
        {
            var farmServerParams = new FarmServerParamHandler(Name, Farm, UseDnsLookup.IsPresent);
            var farmManager = farmServerParams.GetWebFarmManager();

            switch(State)
            {
                case State.Online:
                    farmManager.TakeOnline(WriteObject);
                    break;
                case State.Offline:
                    farmManager.TakeOffline(Force.ToBool(), WriteObject);
                    break;
                case State.Available:
                    farmManager.SetAvailable(WriteObject);
                    break;
                case State.DisallowNewConnections:
                    farmManager.DisallowNewConnections(WriteObject);
                    break;
                case State.Healthy:
                    farmManager.SetHealthy(WriteObject);
                    break;
                case State.Unavailable:
                    farmManager.SetUnavailable(Force.ToBool(), WriteObject);
                    break;
                case State.Unhealthy:
                    farmManager.SetUnhealthy(WriteObject);
                    break;
                default:
                    throw new Exception(string.Format("State [{0}] not supported.", State));
            }
        }

        [Parameter(Mandatory = true, Position = 0, HelpMessage = "Set the new State for the Web Farm Server. " +
                                                                 "Available states are: Online, Offline, Available, " +
                                                                 "Unavailable, Healthy, Unhealthy and DisallowNewConnections")]
        public State State { get; set; }

        [Parameter(Position = 1, ValueFromPipelineByPropertyName = true, HelpMessage = "The name of the Web Farm Server. Support wildcards, but not " +
                                               "together with the UseDnsLookup option.")]
        public string Name { get; set; }

        [Parameter(Position = 2, ValueFromPipelineByPropertyName = true, HelpMessage = "The name of the Web Farm.")]
        public string Farm { get; set; }


        [Parameter(Position = 3, HelpMessage = "Forces the command to set state. Only effective with the Offline " +
                                               "and Unavailable states. If used, the server is not drained (wait for " +
                                               "no active connections).")]
        public SwitchParameter Force { get; set; }

        [Parameter(Position = 4, HelpMessage = "Does DNS lookup to retreive IP addresses for the server (Name). " +
                                               "This is often useful when Web Farm Servers in Web Farms are identified " +
                                               "with IP addresses. By using DNS lookup all Farm Servers with IP belonging " +
                                               "to a specific server will be returned. Note: When UseDnsLookup is used, " +
                                               "wildcards is not supported.")]
        public SwitchParameter UseDnsLookup { get; set; }
    }
}
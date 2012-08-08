using System.Management.Automation;
using ConDep.PowerShell.ApplicationRequestRouting.Infrastructure;
using Lapointe.PowerShell.MamlGenerator.Attributes;

namespace ConDep.PowerShell.ApplicationRequestRouting
{
    [Cmdlet(VerbsCommon.Get, "WebFarm"),
     CmdletDescription("Gets information about Application Request Routing Web Farms."),
     RelatedCmdlets(typeof(GetWebFarmServerCommand), typeof(SetWebFarmServerStateCommand))]
    public class GetWebFarmCommand : Cmdlet
    {
        protected override void ProcessRecord()
        {
            var farmManager = new WebFarmManager(Name, null, false);
            WriteObject(farmManager.Farms, true);
        }

        [Parameter(Position = 0, HelpMessage = "The name of the Web Farm.")]
        public string Name { get; set; }
    }
}
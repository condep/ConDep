using System.Management.Automation;
using Lapointe.PowerShell.MamlGenerator.Attributes;

namespace ConDep.PowerShell.ApplicationRequestRouting
{
    [Cmdlet(VerbsCommon.Get, "WebFarmServer"), 
     CmdletDescription("Gets information about Application Request Routing Web Farm Servers."),
     Example(Code = @"C:\PS>Get-WebFarmServer",
             Remarks = "Gets all Web Farm Servers in all Web Farms.\r\n"),
     Example(Code = @"C:\PS>Get-WebFarmServer -Farm MyWebFarm",
             Remarks = "Gets all Web Farm Servers in the Web Farm MyWebFarm.\r\n"),
     Example(Code = @"C:\PS>Get-WebFarmServer -Name MyServer0*-test",
             Remarks = "Using wildcard to get all Web Farm Servers with name MyServer0*-test. (E.g. MyServer01-test, MyServer02-test etc)\r\n"),
     Example(Code = @"C:\PS>Get-WebFarmServer -Name MyServer01 -UseDnsLookup",
             Remarks = "Does a DNS lookup to find all IP addresses of MyServer01 and gets all Web Farms Servers with those addresses. This is very useful if you have Web Sites using unique IP's and the Web Site IP's are added as Web Farm Servers. This is typically used if you want to set all Web Farm Servers for a specific server to Offline.\r\n"),
     RelatedCmdlets(typeof(GetWebFarmCommand), typeof(SetWebFarmServerStateCommand))]
    public class GetWebFarmServerCommand : Cmdlet
    {
        protected override void ProcessRecord()
        {
            var farmServerParams = new FarmServerParamHandler(Name, Farm, UseDnsLookup.IsPresent);
            var farmManager = farmServerParams.GetWebFarmManager();
            WriteObject(farmManager.FarmServers, true);
        }

        [Parameter(Position = 0, ValueFromPipelineByPropertyName = true , HelpMessage = "The name of the Web Farm Server. Support wildcards, but not " +
                                               "together with the UseDnsLookup option.")]
        [SupportsWildcards]
        public string Name { get; set; }

        [Parameter(Position = 1, ValueFromPipelineByPropertyName = true, HelpMessage = "The name of the Web Farm.")]
        public string Farm { get; set; }

        [Parameter(Position = 2, HelpMessage = "Does DNS lookup to retreive IP addresses for the server (Name). " +
                                               "This is often useful when Web Farm Servers in Web Farms are identified " +
                                               "with IP addresses. By using DNS lookup all Farm Servers with IP belonging " +
                                               "to a specific server will be returned. Note: When UseDnsLookup is used, " +
                                               "wildcards is not supported.")]
        public SwitchParameter UseDnsLookup { get; set; }
    }
}

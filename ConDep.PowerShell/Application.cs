using System.Management.Automation;

namespace ConDep.PowerShell
{
    //Import-Module C:\github\ConDep\ConDep.PowerShell\bin\Debug\ConDep.PowerShell.dll

    //Alias => Application
    [Cmdlet(VerbsCommon.New, "ConDepApplication")]
    public class Application : PSCmdlet
    {
        [Parameter(Mandatory = true, Position = 0)]
        public string Name { get; set; }

        [Parameter(Mandatory = false, Position = 1)]
        public string DependOnInfrastructure { get; set; }

        [Parameter(Mandatory = true, Position = 2)]
        public ScriptBlock DslInput { get; set; }

        protected override void ProcessRecord()
        {
            WriteObject("Name: " + Name);
            WriteObject("DependOnInfrastructure: " + DependOnInfrastructure);
            var result = DslInput.Invoke();
            WriteObject("Result of DslInput: \n");
            WriteObject(result);
            //Create ApplicationArtifact?
        }
    }

    [Cmdlet(VerbsCommon.New, "ConDepToEachServer")]
    public class ToEachServer : PSCmdlet
    {
        protected override void ProcessRecord()
        {
        }       
    }
}

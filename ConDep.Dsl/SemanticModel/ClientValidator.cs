using System.Management.Automation;
using System.Management.Automation.Runspaces;
using ConDep.Dsl.Logging;
using Microsoft.Win32;
using System.Linq;

namespace ConDep.Dsl.SemanticModel
{
    public class ClientValidator : IValidateClient
    {
        public void Validate()
        {
            Logger.LogSectionStart("Validating Client");
            try
            {
                using (var runspace = RunspaceFactory.CreateRunspace())
                {
                    runspace.Open();
                    var ps = PowerShell.Create();
                    ps.Runspace = runspace;

                    using (var pipeline = ps.Runspace.CreatePipeline("set-executionpolicy remotesigned -force"))
                    {
                        pipeline.Commands.AddScript("$psversiontable.psversion.Major");
                        var result = pipeline.Invoke();

                        if (result == null)
                            throw new ConDepClientValidationException("Unable to detect PowerShell version on client! PowerShell version 3.0 or higher is required.");

                        if (result.Count == 1)
                        {
                            dynamic version = result[0];
                            if (version >= 3)
                            {
                                Logger.Info(string.Format("PowerShell version {0} detected on client.", version));
                                return;
                            }
                        }
                        throw new ConDepClientValidationException("Unable to detect PowerShell version on client! PowerShell version 3.0 or higher is required.");
                    }
                }
            }
            finally
            {
                Logger.LogSectionEnd("Validating Client");
            }
        }
    }
}
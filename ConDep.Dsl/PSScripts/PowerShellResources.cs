using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace ConDep.Dsl.PSScripts
{
    public class PowerShellResources
    {
        public static IEnumerable<string> PowerShellScriptResources
        {
            get
            {
                var type = typeof(PowerShellResources);
                var resources = type.Assembly.GetManifestResourceNames();
                var pattern = type.Namespace + @"\..+\.(ps1|psm1)";
                var regex = new Regex(pattern);
                return resources.Where(psPath => regex.Match(psPath).Success);
            }
        } 
    }
}
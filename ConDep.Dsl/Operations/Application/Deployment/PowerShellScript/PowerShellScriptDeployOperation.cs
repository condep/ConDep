using System.Collections.Generic;
using System.IO;
using ConDep.Dsl.Builders;
using ConDep.Dsl.SemanticModel;

namespace ConDep.Dsl.Operations.Application.Deployment.PowerShellScript
{
    public class PowerShellScriptDeployOperation : RemoteCompositeOperation
    {
        private readonly List<string> _scripts = new List<string>();

        public PowerShellScriptDeployOperation(string scriptPath)
        {
            _scripts.Add(scriptPath);
        }

        public PowerShellScriptDeployOperation(IEnumerable<string> scriptPaths)
        {
            _scripts.AddRange(scriptPaths);
        }

        public override void Configure(IOfferRemoteComposition server)
        {
            foreach (var script in _scripts)
            {
                server.Deploy.File(script, @"%temp%\ConDepPowerShellScripts\ConDep\" + Path.GetFileName(script));
            }
        }

        public override string Name
        {
            get { return "Deploy PowerShell Scripts"; }
        }

        public override bool IsValid(Notification notification)
        {
            return _scripts.Count > 0;
        }
    }
}
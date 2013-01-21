using System;
using System.IO;
using System.Text;
using ConDep.Dsl.Builders;
using ConDep.Dsl.SemanticModel;

namespace ConDep.Dsl.Operations.Application.Execution.PowerShell
{
    public class PowerShellProvider : RemoteCompositeOperation
    {
        private int _waitInterval = 30;

        public PowerShellProvider(string scriptOrCommand)
        {
            DestinationPath = scriptOrCommand;
        }

        public PowerShellProvider(FileInfo scriptFile)
        {
            using(var reader = scriptFile.OpenText())
            {
                DestinationPath = reader.ReadToEnd();
            }
        }

        private bool IsScript(string scriptOrCommand)
        {
            return scriptOrCommand.Contains(Environment.NewLine);
        }

        public bool ContinueOnError { get; set; }
        public int WaitIntervalInSeconds { get { return _waitInterval; } set { _waitInterval = value; } }
        public int RetryAttempts { get; set; }

        public bool RequireRemoteLib { get; set; }

        public override void Configure(IOfferRemoteComposition server)
        {
            string libImport = "";

            if (IsScript(DestinationPath))
            {
                var builder = new StringBuilder(DestinationPath);
                builder.Replace(Environment.NewLine, "`" + Environment.NewLine);
                DestinationPath = builder.ToString();
            }

            //var script = AddExitCodeHandlingToScript(DestinationPath);
            //var filePath = CreateScriptFile(script);
            //var destFilePath = CopyScriptToDestination(server, filePath);
            //ExecuteScriptOnDestination(server, destFilePath);

            if(RequireRemoteLib)
            {
                server.Deploy.File(Path.Combine(Path.GetDirectoryName(GetType().Assembly.Location), "ConDep.Remote.dll"), @"%temp%\ConDep.Remote.dll");
                libImport = "Add-Type -Path \"" + @"%temp%\ConDep.Remote.dll" + "\";";
            }
            //elseif($Error.Count -gt 0) {{ Write-Error $Error[0]; exit 1; }} 
            server.ExecuteRemote.DosCommand(string.Format(@"powershell.exe -noprofile -InputFormat none -Command ""& {{ $ErrorActionPreference='stop'; set-executionpolicy remotesigned -force; {0}{1}; if(!$?) {{ exit 1; }} else {{ exit $LASTEXITCODE; }} }}""", libImport, DestinationPath), o => o.ContinueOnError(ContinueOnError).WaitIntervalInSeconds(WaitIntervalInSeconds).RetryAttempts(RetryAttempts));
        }

        //private string AddExitCodeHandlingToScript(string script)
        //{
        //    var builder = new StringBuilder(script);
        //    builder.Insert(0, "$ErrorActionPreference='stop';" + Environment.NewLine);
        //    builder.Append(Environment.NewLine + "exit $LASTEXITCODE");
        //    return builder.ToString();
        //}

        //private void ExecuteScriptOnDestination(ServerConfig server, string destFilePath)
        //{
        //    Configure<ProvideForInfrastructure>(server, po => po.RunCmd(string.Format(@"powershell.exe -InputFormat none -File '{0}'", destFilePath), ContinueOnError, o => o.WaitIntervalInSeconds(WaitInterval)));
        //}

        //private string CopyScriptToDestination(ServerConfig server, string filePath)
        //{
        //    var destFilePath = @"%temp%\" + Path.GetFileName(filePath);
        //    Configure<ProvideForDeployment>(server, c => c.CopyFile(filePath, destFilePath));
        //    return destFilePath;
        //}

        //private string CreateScriptFile(string script)
        //{
        //    var fileName = new Guid().ToString() + ".condep";
        //    var filePath = Path.Combine(Path.GetTempPath(), fileName);
        //    File.WriteAllText(filePath, script);
        //    return filePath;
        //}

        public override string Name
        {
            get { return "PowerShell"; }
        }

        public override bool IsValid(Notification notification)
        {
            var remoteLibExist = true;
            if(RequireRemoteLib)
            {
                remoteLibExist = File.Exists(Path.Combine(Path.GetDirectoryName(GetType().Assembly.Location), "ConDep.Remote.dll"));
            }
            return string.IsNullOrWhiteSpace(SourcePath) && !string.IsNullOrWhiteSpace(DestinationPath) && remoteLibExist;
        }
    }
}